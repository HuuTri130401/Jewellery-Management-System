using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.Features.UserFeatures;
using THT.JMS.Application.IService;
using THT.JMS.Domain.Entities;
using static THT.JMS.Utilities.Enums;
using THT.JMS.Utilities;
using THT.JMS.Application.IRepository;
using Microsoft.EntityFrameworkCore;

namespace THT.JMS.Persistence.Service
{
    public class AuthService : DomainService<Users, UserSearch>, IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork) : base(unitOfWork, mapper)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtToken(Users user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<string> GenerateRefreshToken()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var randomNumber = new byte[32];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(randomNumber);
                        return Convert.ToBase64String(randomNumber);
                    }
                });
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<UserLoginResponseModel> Login(LoginModel request)
        {
            Users user = await Queryable
                .Where(e => e.Deleted == false && (e.UserName == request.UserName))
                .FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.Status == (int)UserStatus.Locked)
                    throw new JMSException("Tài khoản bạn đã bị khóa. Vui lòng liên hệ quản trị!");
                if (user.IsAdmin == false)
                    throw new JMSException("Không có quyền truy cập!");
                var userModel = _mapper.Map<UserModel>(user);
                var token = await GenerateJwtToken(user);
                var refreshToken = await GenerateRefreshToken();

                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(180);
                user.RefreshToken = refreshToken;
                var refreshTokenExpiryTime = user.RefreshTokenExpiryTime;
                _unitOfWork.Repository<Users>().UpdateFieldsSave(user, x => x.RefreshToken, x => x.RefreshTokenExpiryTime);
                await _unitOfWork.SaveAsync();

                return new UserLoginResponseModel()
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = refreshTokenExpiryTime,
                };
            }
            return null;
        }
    }
}
