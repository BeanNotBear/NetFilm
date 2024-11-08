using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.Attributes;

namespace NetFilm.Application.DTOs.AuthDTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Password]
        public string PassWord { get; set; }
    }
}
