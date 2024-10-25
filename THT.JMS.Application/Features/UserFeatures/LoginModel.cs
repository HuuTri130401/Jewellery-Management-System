using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace THT.JMS.Application.Features.UserFeatures
{
    public class LoginModel 
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc nhập")]
        [DefaultValue("admin")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc nhập")]
        [DefaultValue("23312331")]
        public string Password { set; get; }
    }
    public sealed record UserLoginResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
