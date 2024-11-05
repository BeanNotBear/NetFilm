using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetFilm.Persistence.Data;

namespace NetFilm.Persistence
{
	public static class DependencyInjection
	{
		public static void AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<NetFilmDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			});
		}
	}
}
