using Microsoft.AspNetCore.Http;

namespace NetFilm.Application.DTOs.SubtitleDTOs
{
	public class AddSubtitleRequestDto
	{
        public IFormFileCollection Files { get; set; }
        public List<string> SubtitleName { get; set; }
        public Guid MovieId { get; set; }
    }
}
