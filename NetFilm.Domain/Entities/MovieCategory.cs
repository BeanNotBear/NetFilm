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
		public Guid Category_Id { get; set; }
		public Guid Movie_Id { get; set; }
		public Category Category { get; set; }
		public Movie Movie { get; set; }
	}
}
