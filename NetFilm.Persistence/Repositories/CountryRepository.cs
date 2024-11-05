using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;

namespace NetFilm.Persistence.Repositories
{
	public class CountryRepository : BaseRepository<Country, Guid>, ICountryRepository
	{
		public CountryRepository(NetFilmDbContext context) : base(context)
		{
		}
	}
}
