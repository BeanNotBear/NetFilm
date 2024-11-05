using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class Advertise : BaseEntity<Guid>
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string Image { get; set; }
        public Guid CreatedBy { get; set; }
        public User User { get; set; }
    }
}
