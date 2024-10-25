using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using THT.JMS.Application;
using THT.JMS.Persistence;

var builder = WebApplication.CreateBuilder(args);

//============ Config Persistence and Application Project ============//
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureApplication();

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
app.UseAuthentication();

app.MapControllers();

app.Run();
