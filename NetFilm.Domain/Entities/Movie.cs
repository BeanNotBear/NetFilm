using NetFilm.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class Movie : BaseEntity<Guid>
	{
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
		public Guid CountryId { get; set; }
		public Country Country { get; set; }
		public ICollection<MovieCategory> MovieCategories { get; set; }
		public ICollection<MovieParticipant> MovieParticipants { get; set; }
	}
}
