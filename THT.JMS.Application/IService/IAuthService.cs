using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.Features.UserFeatures;
using THT.JMS.Domain.Entities;

namespace THT.JMS.Application.IService
{
    public interface IAuthService : IDomainService<Users, UserSearch>
    {
        Task<string> GenerateRefreshToken();
        Task<string> GenerateJwtToken(Users user);
        Task<UserLoginResponseModel> Login(LoginModel request);
    }
}
