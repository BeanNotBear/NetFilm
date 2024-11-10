using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.AuthDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
using NetFilm.Infrastructure.Services;

namespace NetFilm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthsController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            try
            {
                var userDto = await authService.Register(registerRequestDto);
                if (userDto != null)
                {
                        
                }
                return Ok(userDto);
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { Error = e.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {

            var response = await authService.Login(loginRequestDto);
            return Ok(response);

        }

        [HttpPost]
        [Route("EmailVerification")]
        public async Task<IActionResult> EmailVerification(string email, string code)
        {
            return Ok(await authService.EmailVerification(email, code));
        }
    }
}
