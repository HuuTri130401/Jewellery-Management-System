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
    public class BaseRepository<T> : DomainRepository<T>, IBaseRepository<T> where T : BaseEntity
    {
        public BaseRepository(AppDbContext context) : base(context)
        {
        }
    }
}
