using Microsoft.AspNetCore.Http.Features;
using NetFilm.API.Middlewares;
using NetFilm.Application;
using NetFilm.Domain;
using NetFilm.Infrastructure;
using NetFilm.Persistence;

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

builder.Services.Configure<FormOptions>(x =>
{
	x.ValueLengthLimit = int.MaxValue;
	x.MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024;
});

builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = 5L * 1024 * 1024 * 1024;
});

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
