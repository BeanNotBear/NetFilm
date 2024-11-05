using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetFilm.Infrastructure.Mappers;

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
		}
	}
}
