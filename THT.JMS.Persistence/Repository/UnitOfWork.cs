using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Domain.Common;
using THT.JMS.Persistence.Context;

namespace THT.JMS.Persistence.Repository
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        protected IDbContextTransaction _dbContextTransaction;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            if (_appDbContext != null)
            {
                _appDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                _appDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            }
        }

        public async Task BeginTransactionAsync()
        {
            _dbContextTransaction = await _appDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if(_dbContextTransaction != null)
            {
                await _dbContextTransaction.CommitAsync();
            }
        }

        public abstract IDomainRepository<T> Repository<T>() where T : BaseEntity;

        public async Task RollBackAsync()
        {
            if(_dbContextTransaction != null)
            {
                await _dbContextTransaction.RollbackAsync();
            }
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return _appDbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return _appDbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
