using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Domain.Common;
using THT.JMS.Persistence.Context;

namespace THT.JMS.Persistence.Repository
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        //không cần truy cập từ lớp dẫn xuất, thể hiện tính đóng gói: encapsulation
        //readonly: Biến chỉ được gán một lần (thường là trong constructor của lớp cơ sở)., cố định ko đổi sau khi khởi tạo
        private readonly AppDbContext _appDbContext; 

        //sử dụng protected ở đây là hợp lý vì biến này có thể được các lớp dẫn xuất(như ConcreteUnitOfWork)
        //truy cập nếu cần thiết cho các thao tác giao dịch(BeginTransaction, Commit, Rollback)
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

        //Khai báo abstract trong UnitOfWork nghĩa là lớp UnitOfWork chỉ định nghĩa Repository<T>() nhưng không triển khai.
        //Bất kỳ lớp nào kế thừa từ UnitOfWork(ví dụ: ConcreteUnitOfWork) bắt buộc phải override và triển khai phương thức
        //Repository<T>() để có thể sử dụng.
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
