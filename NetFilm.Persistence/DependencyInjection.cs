using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;
using NetFilm.Persistence.Repositories;

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

			// DI for repository
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<IMovieRepository, MovieRepository>();
			services.AddScoped<ISubtitleRepository, SubtitleRepository>();
			
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<ICommentRepository, CommentRepository>();
		}
	}
}
