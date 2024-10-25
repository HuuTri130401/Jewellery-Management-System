using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using THT.JMS.Application.Features.UserFeatures;
using THT.JMS.Application.IService;
using THT.JMS.Domain.Entities;
using THT.JMS.Utilities;

namespace THT.JMS.API.Controllers
{
    [Description("User Management")]
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.GetRequiredService<IUserService>();
        }

        [HttpGet]
        [Description("Xem danh sách người dùng")]
        public async Task<AppDomainResult> Get([FromQuery] UserSearch search)
        {

            PagedList<UserModel> pagedList = await _userService.GetPagedListUsers(search);

            return new AppDomainResult
            {
                success = true,
                resultCode = (int)HttpStatusCode.OK,
                data = pagedList
            };
        }
    }
}
