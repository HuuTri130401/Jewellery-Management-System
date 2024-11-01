using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using THT.JMS.Application;
using THT.JMS.Persistence;

var builder = WebApplication.CreateBuilder(args);

//============ Config Persistence and Application Project ============//
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureApplication();

//============ AutoMapper ============//
builder.Services.AddAutoMapper(typeof(Program));

//============ CORS ============//
builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "THT JMS API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


// Thiết lập HTTPS redirection chỉ khi không ở môi trường phát triển (Development)
//if (!builder.Environment.IsDevelopment())
//{
//    builder.Services.AddHttpsRedirection(options =>
//    {
//        //AddHttpsRedirection: Cấu hình ứng dụng để tự động chuyển hướng HTTP sang HTTPS trong môi trường sản xuất
//        options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
//        options.HttpsPort = 443;
//    });
//}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod();
});

//UseHttpsRedirection: Kích hoạt middleware chuyển hướng sang HTTPS.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
