using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Infrastructure.Services;

namespace NetFilm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<UserDto>> CreateUser(AddUserRequestDto request)
        {
            var user = await userService.Add(request);
            return Ok(user);
        }
    }
}
