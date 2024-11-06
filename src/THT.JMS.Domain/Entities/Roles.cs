using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Domain.Common;

namespace THT.JMS.Domain.Entities
{
    public class Roles : BaseEntity
    {
        public string RoleName { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
