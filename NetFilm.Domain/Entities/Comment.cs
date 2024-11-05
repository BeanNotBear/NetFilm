using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class Comment : BaseEntity<Guid>
	{
        public string Content { get; set; }
        public Guid MovieId { get; set; }
        public Guid ReplyId { get; set; }
        public Guid UserId { get; set; }
        public Movie Movie { get; set; }
		public ICollection<Comment> Comments { get; set; }
        public User User { get; set; }

    }
}
