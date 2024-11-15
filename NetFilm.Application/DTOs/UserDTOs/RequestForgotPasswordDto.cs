using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.UserDTOs
{
    public class RequestForgotPasswordDto
    {
        public string Email { get; set; } = string.Empty;
    }
}
