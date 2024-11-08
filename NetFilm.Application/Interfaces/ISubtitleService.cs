using NetFilm.Application.DTOs.SubtitleDTOs;


namespace NetFilm.Application.Interfaces
{
	public interface ISubtitleService
	{
		Task<SubtitleDto> AddSubtitle(string subtitleName, string subtitleUrl, Guid movieId);
	}
}
