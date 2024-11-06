using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Domain.Common;
using THT.JMS.Utilities;

namespace THT.JMS.Application.IRepository
{
    public interface IDomainRepository<T> where T : BaseEntity 
    {
        IQueryable<T> GetQueryable();
        void Create(T entity);
        Task CreateAsync(T entity);
        void Create(IList<T> entities);
        Task CreateAsync(IList<T> entities);
        void Update(T entity);
        void SystemUpdate(T entity);
        void Update(List<T> entities);
        void Delete(T entity);
        void Delete(IList<T> entities);
        void Attach(T item);
        void Detach(T entity);
        void SetEntityState(T item, EntityState entityState);
        EntityEntry<T> Entry(T item);
        void LoadReference(T item, params string[] property);
        void LoadCollection(T item, params string[] property);
        int ExecuteNonQuery(string commandText, SqlParameter[] sqlParameters);
        int ExecuteNonQuery(string commandText);
        DataTable ExcuteQuery(string commandText, SqlParameter[] sqlParameters);
        Task<PagedList<T>> ExcuteQueryPagingAsync(string commandText, SqlParameter[] sqlParameters);
        Task<DataTable> ExcuteQueryAsync(string commandText, SqlParameter[] sqlParameters);
        bool UpdateFieldsSave(T entity, params Expression<Func<T, object>>[] includeProperties);
        Task<IList<T>> ExcuteStoreAsync(string commandText, SqlParameter[] sqlParameters);
        Task<object> ExcuteStoreGetValue(string commandText, SqlParameter[] sqlParameters, string outputName);
        Task<bool> UpdateFieldsSaveAsync(T entity, params Expression<Func<T, object>>[] includeProperties);
    }
}
