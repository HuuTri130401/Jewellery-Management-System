using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Application.Features.UserFeatures;
using THT.JMS.Application.IRepository;
using THT.JMS.Application.IService;
using THT.JMS.Domain.Common;
using THT.JMS.Domain.Entities;
using THT.JMS.Persistence.Context;
using THT.JMS.Persistence.Repository;
using THT.JMS.Persistence.Service;

namespace THT.JMS.Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("JMSDbContext");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IUnitOfWork, ConcreteUnitOfWork>();
            services.AddScoped(typeof(IDomainRepository<>), typeof(DomainRepository<>));

            #region Entity_Repo
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IDomainService<Users, BaseSearch>, DomainService<Users, BaseSearch>>();
            services.AddScoped<IAuthService, AuthService>();
            #endregion Entity_Repo

            // Cấu hình JWT
            var jwtSettings = configuration.GetSection("JwtSettings");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
                };
            });
        }
    }
}
