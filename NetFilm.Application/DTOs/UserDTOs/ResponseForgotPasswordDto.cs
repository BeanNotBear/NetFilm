using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.UserDTOs
{
    public class ResponseForgotPasswordDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
