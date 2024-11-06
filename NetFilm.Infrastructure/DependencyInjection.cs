using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetFilm.Application.Interfaces;
using NetFilm.Infrastructure.Mappers;
using NetFilm.Infrastructure.Services;

namespace NetFilm.Infrastructure
{
	public static class DependencyInjection
	{
		public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(options =>
			{
				options.AddProfile(typeof(ProfileMapper));
			});

			// DI for service
			services.AddScoped<ICountryService, CountryService>();

			// Injected User Service
			services.AddTransient<ICountryService, CountryService>();
			services.AddScoped<ICategoryService, CategoryService>();
		}
	}
}
