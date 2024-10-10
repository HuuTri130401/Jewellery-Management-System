using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Domain.Entities;

namespace THT.JMS.Application.IRepository
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllAsync();
    }
}
