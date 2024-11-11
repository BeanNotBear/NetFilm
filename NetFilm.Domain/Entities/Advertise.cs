using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column("CreatedBy")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
