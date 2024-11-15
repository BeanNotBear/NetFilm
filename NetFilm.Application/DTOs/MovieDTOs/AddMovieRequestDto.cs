using Microsoft.AspNetCore.Http;
using NetFilm.Application.Attributes;
using NetFilm.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace NetFilm.Application.DTOs.MovieDTOs
{
	public class AddMovieRequestDto
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		[Range(0, 2)]
		public Quality Quality { get; set; }

		[Required]
		[Range(0, int.MaxValue)]
		public int Allowing_Age { get; set; }

		[Required]
		[NowDateProperty]
		public DateTime Release_Date { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Duration { get; set; }

		[Required]
		public Guid CountryId { get; set; }

		[Required]
        public IFormFile File { get; set; }

		[Required]
		public List<Guid> CategoryIds { get; set; }
	}
}
