using Microsoft.AspNetCore.Http;
using NetFilm.Domain.Common;

namespace NetFilm.Application.DTOs.MovieDTOs
{
	public class UpdateMovieRequestDto
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? Thumbnail { get; set; }
		public MovieStatus? Status { get; set; }
		public Quality? Quality { get; set; }
		public string? Movie_Url { get; set; }
		public int? Allowing_Age { get; set; }
		public DateTime? Release_Date { get; set; }
		public int? Duration { get; set; }
		public Guid? CountryId { get; set; }
        public IFormFile? Movie { get; set; }
		public IFormFile? ThumbnailImage { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid>? ParticipantIds { get; set; }
    }
}
