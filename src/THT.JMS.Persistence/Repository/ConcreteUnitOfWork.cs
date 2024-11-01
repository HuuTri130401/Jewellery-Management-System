using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Persistence.Context;

namespace THT.JMS.Persistence.Repository
{
    public class ConcreteUnitOfWork : UnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public ConcreteUnitOfWork(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IDomainRepository<T> Repository<T>()
        {
            return new BaseRepository<T>(_dbContext);
        }
    }
}
