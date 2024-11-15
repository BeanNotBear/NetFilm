using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.Attributes;

namespace NetFilm.Application.DTOs.UserDTOs
{
    public class ResetPasswordRequestDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Password]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Password]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
