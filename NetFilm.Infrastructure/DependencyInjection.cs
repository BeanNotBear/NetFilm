using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Interfaces;
using NetFilm.Infrastructure.Mappers;
using NetFilm.Infrastructure.Services;
using NetFilm.Persistence.Repositories;


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
            services.AddTransient<ICountryService, CountryService>();


            
            services.AddTransient<IParticipantService, ParticipantService>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();

            services.AddTransient<IVoteService, VoteService>();
            services.AddTransient<IVoteRepository, VoteRepository>();
        }
    }
}
