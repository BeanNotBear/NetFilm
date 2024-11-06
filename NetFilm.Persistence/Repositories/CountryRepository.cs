using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;

namespace NetFilm.Persistence.Repositories
{
	public class CountryRepository : BaseRepository<Country, Guid>, ICountryRepository
	{
		private readonly NetFilmDbContext _context;
		public CountryRepository(NetFilmDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			var country = await _context.Countries.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
			return country != null;
		}

		public async Task<Country> SoftDeleteAsync(Guid id)
		{
			var entity = await _context.Countries.FindAsync(id);
			entity.IsDelete = true;	
			await _context.SaveChangesAsync();
			return entity;
		}
	}
}
