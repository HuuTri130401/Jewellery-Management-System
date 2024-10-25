using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Application.IService;
using THT.JMS.Domain.Entities;
using THT.JMS.Persistence.Service;
using THT.JMS.Utilities;

namespace THT.JMS.Application.Features.UserFeatures
{
    public class UserService : DomainService<Users, UserSearch>, IUserService
    {
        private IConfiguration _configuaration;

        public UserService(IConfiguration configuaration, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _configuaration = configuaration;
        }

        public async Task<PagedList<UserModel>> GetPagedListUsers(UserSearch baseSearch)
        {
            PagedList<Users> listUsers = await GetPagedListData(baseSearch);
            PagedList<UserModel> listUsersModel = _mapper.Map<PagedList<UserModel>>(listUsers);
            return listUsersModel;
        }

        protected override string GetStoreProcName()
        {
            return "GetPagedData_User";
        }
    }
}
