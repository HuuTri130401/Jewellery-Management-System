using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Domain.Common;

namespace THT.JMS.Application.IRepository
{
    public interface IBaseRepository<T> : IDomainRepository<T> where T : BaseEntity
    {
    }
}
