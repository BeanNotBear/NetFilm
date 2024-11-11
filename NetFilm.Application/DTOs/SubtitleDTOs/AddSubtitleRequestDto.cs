using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NetFilm.Application.DTOs.SubtitleDTOs
{
	public class AddSubtitleRequestDto
	{
        public IFormFileCollection Files { get; set; }
        public List<string> SubtitleName { get; set; }

        [Required]
        public Guid MovieId { get; set; }
    }
}
