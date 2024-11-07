using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;

namespace NetFilm.Persistence.Repositories
{
	public class SubtitleRepository : BaseRepository<Subtitle, Guid>, ISubtitleRepository
	{
		public SubtitleRepository(NetFilmDbContext context) : base(context)
		{
		}
	}
}
