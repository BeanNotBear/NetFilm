using Amazon.S3;
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

			// DI for AWS Service
			services.AddDefaultAWSOptions(configuration.GetAWSOptions());
			services.AddAWSService<IAmazonS3>();
			services.AddScoped<IAWSService, AWSService>();

			// DI for service
			services.AddScoped<ICountryService, CountryService>();

			// Injected User Service
			services.AddTransient<ICountryService, CountryService>();
			services.AddScoped<ICategoryService, CategoryService>();
		}
	}
}
