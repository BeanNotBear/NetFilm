using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.MovieDTOs
{
	public class MovieViewerDto
	{
		public Guid Id { get; set; }
		public string Thumbnail { get; set; }
		public string Name { get; set; }
		public DateTime Release_Date { get; set; }
		public float Average_Star { get; set; }
	}
}
