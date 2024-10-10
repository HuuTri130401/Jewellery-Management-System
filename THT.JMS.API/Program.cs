using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using THT.JMS.Persistence;
using THT.JMS.Persistence.Context;
using THT.JMS.Application;


var builder = WebApplication.CreateBuilder(args);

//============ Config Persistence and Application Project ============//
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureApplication();

//============ Connect DB ============//
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration["ConnectionStrings:JMSDbContext"]);
//    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//});

//============ AutoMapper ============//
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
