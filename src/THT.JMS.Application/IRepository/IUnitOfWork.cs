using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Domain.Common;

namespace THT.JMS.Application.IRepository
{
    public interface IUnitOfWork
    {
        IDomainRepository<T> Repository<T>() where T : BaseEntity;
        void Save();
        Task SaveAsync();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        Task BeginTransactionAsync();
        Task RollBackAsync();
        Task CommitAsync();
    }
}
