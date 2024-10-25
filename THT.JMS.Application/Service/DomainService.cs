using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Application.IService;
using THT.JMS.Domain.Common;
using THT.JMS.Utilities;

namespace THT.JMS.Persistence.Service
{
    public abstract class DomainService<E, T> : IDomainService<E, T> where E : BaseEntity where T : BaseSearch, new()
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        public bool IsUseStore = true;

        protected DomainService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected IQueryable<E> Queryable
        {
            get
            {
                return _unitOfWork.Repository<E>().GetQueryable().AsNoTracking();
            }
        }
        public IQueryable<E> Query => Queryable;

        public async Task<bool> CreateAsync(E item)
        {
            return await CreateAsync(new List<E> { item });
        }

        public virtual async Task<bool> CreateAsync(IList<E> items)
        {
            foreach (var model in items)
            {
                foreach (PropertyInfo item in model.GetType().GetProperties())
                {
                    var value = item.GetValue(model);
                    var typeofItem = item.PropertyType.GenericTypeArguments.FirstOrDefault();
                    if (typeofItem == typeof(Boolean))
                    {
                        item.SetValue(model, value ?? false);
                    }
                    else if (typeofItem == typeof(Int32) || item.PropertyType == typeof(Double))
                    {
                        item.SetValue(model, value ?? 0);
                    }
                    else if (item.PropertyType == typeof(String))
                    {
                        item.SetValue(model, value ?? "");
                    }
                }
                model.Created = DateTime.UtcNow.AddHours(7);
                model.Deleted = false;
                if (model.CreatedBy == null || model.CreatedBy == Guid.Empty)
                {
                    model.CreatedBy = LoginContext.Instance?.CurrentUser?.UserId;
                }
                await _unitOfWork.Repository<E>().CreateAsync(model);
            }
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<E> GetByIdAsync(Guid id)
        {
            return await Queryable
                .Where(e => e.Id == id && e.Deleted == false)
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<E> GetByIdAsync(Guid id, IConfigurationProvider mapperConfiguration)
        {
            var queryable = Queryable
                .Where(e => e.Deleted == false && e.Id == id);
            if (mapperConfiguration != null)
                queryable = queryable.ProjectTo<E>(mapperConfiguration);
            return await queryable.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<PagedList<E>> GetPagedListData(T baseSearch)
        {
            PagedList<E> pagedList = new PagedList<E>();
            if (IsUseStore)
            {
                SqlParameter[] parameters = GetSqlParameters(baseSearch);
                pagedList = await _unitOfWork.Repository<E>()
                    .ExcuteQueryPagingAsync(this.GetStoreProcName(), parameters);
                pagedList.pageIndex = baseSearch.PageIndex;
                pagedList.pageSize = baseSearch.PageSize;
            }
            else
            {
                if (baseSearch.PageSize == 0 && baseSearch.PageIndex == 0)
                {
                    baseSearch.PageSize = int.MaxValue;
                    baseSearch.PageIndex = 1;
                }

                int skip = (baseSearch.PageIndex - 1) * baseSearch.PageSize;
                int take = baseSearch.PageSize;

                var items = this.Queryable.Where(GetExpression(baseSearch));
                decimal itemCount = items.Count();
                pagedList = new PagedList<E>()
                {
                    totalItem = (int)itemCount,
                    items = await items.OrderByDescending(x => x.Created).Skip(skip).Take(take).ToListAsync(),
                    pageIndex = baseSearch.PageIndex,
                    pageSize = baseSearch.PageSize,
                };
            }
            return pagedList;
        }
        protected virtual Expression<Func<E, bool>> GetExpression(T baseSearch)
        {
            return e => !(bool)e.Deleted;
        }
        protected virtual string GetStoreProcName()
        {
            return string.Empty;
        }
        protected virtual SqlParameter[] GetSqlParameters(T baseSearch)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (PropertyInfo prop in baseSearch.GetType().GetProperties())
            {
                Type type = prop.PropertyType;
                var name = prop.Name;
                var value = prop.GetValue(baseSearch, null);
                //nếu param dạng list thì convert to string. lưu ý value khác null mới convert được.
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && value != null)
                {
                    List<object> result = ((IEnumerable)value).Cast<object>().ToList();
                    string arrayString = string.Join(",", result.ToArray());
                    sqlParameters.Add(new SqlParameter(name, arrayString));
                }
                else
                {
                    sqlParameters.Add(new SqlParameter(name, value));
                }
            }
            SqlParameter[] parameters = sqlParameters.ToArray();
            return parameters;
        }
        public async Task<E> GetSingleAsync(Expression<Func<E, bool>> expression)
        {
            return await _unitOfWork.Repository<E>()
                .GetQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(E item)
        {
            return await UpdateAsync(new List<E> { item });
        }

        public async Task<bool> UpdateAsync(IList<E> items)
        {
            foreach (var model in items)
            {
                var entity = await Queryable
                 .AsNoTracking()
                 .Where(e => e.Id == model.Id && e.Deleted == false)
                 .FirstOrDefaultAsync();

                if (entity != null)
                {
                    foreach (PropertyInfo item_old in entity.GetType().GetProperties())
                    {
                        foreach (PropertyInfo item_new in model.GetType().GetProperties())
                        {
                            if (item_old.Name == item_new.Name)
                            {
                                var value_old = item_old.GetValue(entity);
                                var value_new = item_new.GetValue(model);
                                if (value_old != value_new)
                                {
                                    item_old.SetValue(entity, value_new ?? value_old);
                                }
                                break;
                            }
                        }
                    }
                    entity.UpdatedBy = LoginContext.Instance.CurrentUser == null ? Guid.Empty : LoginContext.Instance.CurrentUser.UserId;
                    _unitOfWork.Repository<E>().Update(entity);
                }
            }
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateFieldAsync(IList<E> items, params Expression<Func<E, object>>[] includeProperties)
        {
            foreach (var item in items)
            {
                var exists = await Queryable
                 .AsNoTracking()
                 .Where(e => e.Id == item.Id && e.Deleted == false)
                 .FirstOrDefaultAsync();

                if (exists != null)
                {
                    exists = _mapper.Map<E>(item);
                    _unitOfWork.Repository<E>().UpdateFieldsSave(exists, includeProperties);
                }
            }
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateFieldAsync(E item, params Expression<Func<E, object>>[] includeProperties)
        {
            return await UpdateFieldAsync(new List<E> { item }, includeProperties);
        }
    }
}
