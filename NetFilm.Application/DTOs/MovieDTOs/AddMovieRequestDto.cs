using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public Quality Quality { get; set; }

		[Required]
		public int Allowing_Age { get; set; }

		[Required]
		[NowDateProperty]
		public DateTime Release_Date { get; set; }

		[Required]
		public int Duration { get; set; }

		[Required]
		public Guid CountryId { get; set; }

		[Required]
		public string CategoryIds { get; set; }

		[Required]
		public string ParticipantIds { get; set; }
	}
}
