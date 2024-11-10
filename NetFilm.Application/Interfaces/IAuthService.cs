using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.DTOs.AuthDTOs;
using NetFilm.Application.DTOs.UserDTOs;

namespace NetFilm.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<string> EmailVerification(string email, string code);
    }
}
