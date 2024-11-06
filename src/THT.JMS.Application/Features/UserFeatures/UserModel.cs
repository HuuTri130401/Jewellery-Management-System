using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THT.JMS.Application.Features.UserFeatures
{
    public class UserModel
    {
        public string Code { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string IdentityCard { get; set; }
        public DateTime? IdentityCardDate { get; set; }
        public string IdentityCardAddress { get; set; }
        public int? Gender { get; set; }
    }
}
