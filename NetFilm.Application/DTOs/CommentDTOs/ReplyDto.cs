using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.CommentDTOs
{
    public class ReplyDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public string Movie { get; set; }
        public DateTime Date { get; set; }
        public Guid CommentId { get; set; }
        public bool IsDelete { get; set; }
    }
}
