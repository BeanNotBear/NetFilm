using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Entities;

namespace NetFilm.Domain.Interfaces
{
    public interface ISubtitleRepository : IBaseRepository<Subtitle, Guid>
    {
		Task<Subtitle> GetSubtitlebyId(Guid SubtitleId);
	}
}
