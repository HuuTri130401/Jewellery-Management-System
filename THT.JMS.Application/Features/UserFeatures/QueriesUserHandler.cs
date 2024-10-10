using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.IRepository;
using THT.JMS.Application.ResponseModels;
using THT.JMS.Application.SearchModels;

namespace THT.JMS.Application.Features.UserFeatures
{
    public class QueriesUserHandler : IRequestHandler<UserSearch, List<UserModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public QueriesUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserModel>> Handle(UserSearch request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            var responseUsers = _mapper.Map<List<UserModel>>(users);
            return responseUsers.ToList();
        }
    }
}
