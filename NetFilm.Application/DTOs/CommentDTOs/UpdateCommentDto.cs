﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.CommentDTOs
{
    public class UpdateCommentDto
    {
        [Required]
        public string Content { get; set; }
    }
}
