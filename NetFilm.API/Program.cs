using NetFilm.API.Middlewares;
using NetFilm.Application;
using NetFilm.Application.Interfaces;
using NetFilm.Domain;
using NetFilm.Domain.Interfaces;
using NetFilm.Infrastructure;
using NetFilm.Infrastructure.Services;
using NetFilm.Persistence;
using NetFilm.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomainService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddPersistenceService(builder.Configuration);
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 1000 * 1024 * 1024);
// Ðãng k? repository



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
