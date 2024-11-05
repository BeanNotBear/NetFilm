using NetFilm.Domain.Entities;

namespace NetFilm.Domain.Interfaces
{
	public interface ICountryRepository : IBaseRepository<Country, Guid>
	{
		Task<bool> ExistsByNameAsync(string name);
		Task<Country> SoftDeleteAsync(Guid id);
	}
}
