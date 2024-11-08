using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Domain.Common;

namespace NetFilm.Application.DTOs.MovieDTOs
{
	public class MovieDto
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
		public CountryDto Country { get; set; }
    }
}
