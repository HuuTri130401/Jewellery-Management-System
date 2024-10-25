using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.Features.UserFeatures;
using THT.JMS.Domain.Entities;
using THT.JMS.Utilities;

namespace THT.JMS.Application.IService
{
    public interface IUserService : IDomainService<Users, UserSearch>
    {
        Task<PagedList<UserModel>> GetPagedListUsers(UserSearch baseSearch);
    }
}
