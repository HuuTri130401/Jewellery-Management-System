using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Domain.Common;
using THT.JMS.Persistence.Context;
using THT.JMS.Utilities;

namespace THT.JMS.Persistence.Repository
{
    public class DomainRepository<T> : IDomainRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public DomainRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual void SetEntityState(T item, EntityState entityState)
        {
            _context.Entry(item).State = entityState;
        }

        public virtual void Attach(T item)
        {
            _context.Set<T>().Attach(item);
            _context.Entry<T>(item).State = EntityState.Unchanged;
        }

        public virtual void Create(T entity)
        {
            var user = LoginContext.Instance.CurrentUser;
            if (user != null)
            {
                entity.CreatedBy = user.UserId;
            }
            _context.Set<T>().Add(entity);
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Create(IList<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public virtual async Task CreateAsync(IList<T> entities)
        {   
            await _context.Set<T>().AddRangeAsync(entities);
        }
        public virtual void Update(T entity)
        {
            var user = LoginContext.Instance.CurrentUser;
            if (user != null)
            {
                entity.UpdatedBy = user.UserId;
            }
            _context.Set<T>().Update(entity);
        }

        public virtual void SystemUpdate(T entity)
        {   
            _context.Set<T>().Update(entity);
        }

        public virtual bool UpdateFieldsSave(T entity, params Expression<Func<T, object>>[] includeProperties)
        {
            var dbEntry = _context.Entry(entity);

            foreach (var includeProperty in includeProperties)
            {
                dbEntry.Property(includeProperty).IsModified = true;
            }
            _context.SaveChanges();
            return true;
        }
        /// <summary>
        /// Update Field entity
        /// </summary>
        public virtual async Task<bool> UpdateFieldsSaveAsync(T entity, params Expression<Func<T, object>>[] includeProperties)
        {
            return await Task.Run(() =>
            {
                var dbEntry = _context.Entry(entity);

                foreach (var includeProperty in includeProperties)
                {
                    dbEntry.Property(includeProperty).IsModified = true;
                }
                _context.SaveChanges();
                return true;
            });
        }

        public virtual void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void Delete(IList<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public virtual void LoadReference(T item, params string[] property)
        {
            foreach (var prop in property)
            {
                _context.Entry(item).Reference(prop).Load();
            }
        }


        public virtual void LoadCollection(T item, params string[] property)
        {
            foreach (var prop in property)
            {
                _context.Entry(item).Collection(prop).Load();
            }
        }

        /// <summary>
        /// Lấy danh sách phân trang
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public virtual Task<PagedList<T>> ExcuteQueryPagingAsync(string commandText, SqlParameter[] sqlParameters)
        {
            return Task.Run(() =>
            {
                PagedList<T> pagedList = new PagedList<T>();
                DataTable dataTable = new DataTable();
                SqlConnection connection = null;
                SqlCommand command = null;
                try
                {
                    connection = (SqlConnection)_context.Database.GetDbConnection();
                    command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = commandText;
                    command.Parameters.AddRange(sqlParameters);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataTable);
                    pagedList.items = MappingDataTable.ConvertToList<T>(dataTable);
                    if (pagedList.items != null && pagedList.items.Any())
                        pagedList.totalItem = pagedList.items.FirstOrDefault().TotalItem;
                    return pagedList;
                }
                finally
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                        connection.Close();

                    if (command != null)
                        command.Dispose();
                }
            });
        }

        public async Task<object> ExcuteStoreGetValue(string commandText, SqlParameter[] sqlParameters, string outputName)
        {
            return await Task.Run(() =>
            {
                object obj = new object();
                DataTable dataTable = new DataTable();
                SqlConnection connection = null;
                SqlCommand command = null;
                try
                {
                    connection = (SqlConnection)_context.Database.GetDbConnection();
                    command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = commandText;
                    command.Parameters.AddRange(sqlParameters);
                    command.Parameters[outputName].Direction = ParameterDirection.Output;
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataTable);
                    var objectValue = command.Parameters[outputName].Value;
                    if (objectValue != null)
                        obj = objectValue.ToString();
                    return obj;
                }
                finally
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                        connection.Close();

                    if (command != null)
                        command.Dispose();
                }
            });
        }

        public async Task<IList<T>> ExcuteStoreAsync(string commandText, SqlParameter[] sqlParameters)
        {
            return await Task.Run(() =>
            {
                IList<T> listData = new List<T>();
                DataTable dataTable = new DataTable();
                SqlConnection connection = null;
                SqlCommand command = null;
                try
                {
                    connection = (SqlConnection)_context.Database.GetDbConnection();
                    command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = commandText;
                    command.Parameters.AddRange(sqlParameters);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataTable);
                    listData = MappingDataTable.ConvertToList<T>(dataTable);
                    return listData;
                }
                finally
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                        connection.Close();

                    if (command != null)
                        command.Dispose();
                }
            });
        }

        public Task<DataTable> ExcuteQueryAsync(string commandText, SqlParameter[] sqlParameters)
        {
            return Task.Run(() =>
            {
                DataTable dataTable = new DataTable();
                SqlConnection connection = null;
                SqlCommand command = null;
                try
                {
                    connection = (SqlConnection)_context.Database.GetDbConnection();
                    command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = commandText;
                    command.Parameters.AddRange(sqlParameters);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable;
                }
                finally
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                        connection.Close();

                    if (command != null)
                        command.Dispose();
                }
            });
        }

        public DataTable ExcuteQuery(string commandText, SqlParameter[] sqlParameters)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)_context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                command.Parameters.AddRange(sqlParameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public int ExecuteNonQuery(string commandText, SqlParameter[] sqlParameters)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)_context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                command.Parameters.AddRange(sqlParameters);
                return command.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public int ExecuteNonQuery(string commandText)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = (SqlConnection)_context.Database.GetDbConnection();
                command = connection.CreateCommand();
                connection.Open();
                command.CommandText = commandText;
                return command.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (command != null)
                    command.Dispose();
            }
        }

        public EntityEntry<T> Entry(T item)
        {
            EntityEntry<T> entityEntry = _context.Entry(item);
            return entityEntry;
        }

        public void Update(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
    }
}
