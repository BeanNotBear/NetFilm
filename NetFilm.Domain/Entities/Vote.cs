using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class Vote : BaseEntity<Guid>
	{
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
}
}
