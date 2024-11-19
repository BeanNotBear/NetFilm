using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;

namespace NetFilm.Persistence.Repositories
{
	public class SubtitleRepository : BaseRepository<Subtitle, Guid>, ISubtitleRepository
	{
		private readonly NetFilmDbContext _context;
		public SubtitleRepository(NetFilmDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Subtitle> GetSubtitlebyId(Guid SubtitleId)
		{
			var subtitle = await _context.Subtitles.FirstOrDefaultAsync(s => s.Id == SubtitleId);
			return subtitle;
		}
	}
}
