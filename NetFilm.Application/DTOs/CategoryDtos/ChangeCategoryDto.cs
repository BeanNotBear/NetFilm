using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.CategoryDtos
{
    public class ChangeCategoryDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
