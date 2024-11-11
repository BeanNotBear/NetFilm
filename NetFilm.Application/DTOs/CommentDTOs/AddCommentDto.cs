using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.CommentDTOs
{
    public class AddCommentDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid MovieId { get; set; }

    }
}
