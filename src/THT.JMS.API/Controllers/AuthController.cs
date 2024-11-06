using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using THT.JMS.Application.Features.UserFeatures;
using THT.JMS.Application.IService;
using THT.JMS.Utilities;

namespace THT.JMS.API.Controllers
{
    [Description("Auth Management")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        
        public AuthController(IServiceProvider serviceProvider)
        {
            _authService = serviceProvider.GetRequiredService<IAuthService>();
        }

        [HttpPost("login")]
        public async Task<AppDomainResult> Login(LoginModel loginModel)
        {
            var result = await _authService.Login(loginModel);

            return new AppDomainResult
            {
                success = true,
                resultMessage = "Login thành công",
                data = result, 
                resultCode = (int)HttpStatusCode.OK
            };
        }
    }
}
