using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Entities
{
	public class Comment : BaseEntity<Guid>
	{
        public string Content { get; set; }

        [Column(TypeName = "Date")]
        public DateOnly Date { get; set; }
        public Guid MovieId { get; set; }

        [Column("ReplyId")]
        public Guid? CommentId { get; set; }
        public Guid UserId { get; set; }
        public Movie Movie { get; set; }

		[DeleteBehavior(DeleteBehavior.NoAction)]
		public ICollection<Comment> Comments { get; set; }
        public User User { get; set; }

    }
}
