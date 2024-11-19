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
        Task<bool> EmailVerification(VerifyEmailDto verifyEmailDto);
        Task<ResponseForgotPasswordDto> ForgotPassword(RequestForgotPasswordDto requestForgotPasswordDto);
        Task<bool> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<bool> ResendEmail(ResendEmailDto resendEmailDto);
    }
}
