using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NetFilm.Application.DTOs.SubtitleDTOs
{
	public class AddSubtitleRequestDto
	{
        public IFormFile File { get; set; }
        public string SubtitleName { get; set; }

        [Required]
        public Guid MovieId { get; set; }
    }
}
