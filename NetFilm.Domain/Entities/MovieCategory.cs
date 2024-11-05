using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class MovieCategory
	{
		public Guid CategoryId { get; set; }
		public Guid MovieId { get; set; }
		public Category Category { get; set; }
		public Movie Movie { get; set; }
	}
}
