using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Domain.Entities;
using THT.JMS.Persistence.Context;

namespace THT.JMS.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbContext;

        public UserRepository(AppDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Users>> GetAllAsync()
        {
            return await this.dbContext.Users.ToListAsync();
        }
    }
}
