using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using THT.JMS.Application.ResponseModels;
using THT.JMS.Application.SearchModels;
using THT.JMS.Utilities;

namespace THT.JMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<AppDomainResult> GetAll(CancellationToken cancellationToken)
        {
            // Gửi yêu cầu qua MediatR để lấy tất cả UserModel
            var response = await _mediator.Send(new UserSearch(), cancellationToken);

            return new AppDomainResult
            {
                success = true,
                resultMessage = "Lấy danh sách người dùng thành công",
                data = response, // Trả về kết quả từ Handler thông qua MediatR
                resultCode = (int)HttpStatusCode.OK
            };
        }
    }
}
