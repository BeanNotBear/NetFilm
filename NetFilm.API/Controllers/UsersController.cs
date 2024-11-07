using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Application.Exceptions;
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
        public async Task<ActionResult<UserDto>> CreateUser(AddUserRequestDto addUserRequestDto)
        {
            try
            {
                var userDto = await userService.Add(addUserRequestDto);
                return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
            }catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }catch(ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var userDto = await userService.GetById(id);
                return Ok(userDto);
            }catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await userService.GetAll());
        }
    }
}
