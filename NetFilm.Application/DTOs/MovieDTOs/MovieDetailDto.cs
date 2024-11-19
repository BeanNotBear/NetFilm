using NetFilm.Application.DTOs.CommentDTOs;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.MovieCategoryDtos;
using NetFilm.Application.DTOs.ParticipantDTOs;
using NetFilm.Application.DTOs.SubtitleDTOs;
using NetFilm.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.MovieDTOs
{
	public class MovieDetailDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Thumbnail { get; set; }
		public MovieStatus Status { get; set; }
		public Quality Quality { get; set; }
		public float Average_Star { get; set; }
		public string Movie_Url { get; set; }
		public int Allowing_Age { get; set; }
		public DateTime Release_Date { get; set; }
		public int Duration { get; set; }
		public int TotalViews { get; set; }
		public int TotalPeopleVote { get; set; }
		public CountryDto Country { get; set; }
		public ICollection<SubtitleDto> Subtitles { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public ICollection<ParticipantDto> Participants { get; set; }

    }
}
