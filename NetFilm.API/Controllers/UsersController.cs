using Microsoft.AspNetCore.Authorization;
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await userService.GetAll());
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            try
            {
                var userDto = await userService.Update(id, updateUserRequestDto);
                return Ok(userDto);
            }catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("PageResult")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUserPageResult([FromQuery] UserQueryParams userQueryParams)
        {
            try
            {
                return Ok(await userService.GetUserPagedResult(userQueryParams));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id:Guid}/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromRoute] Guid id, [FromBody] PasswordUpdateParam passwordUpdateParam)
        {
            try
            {
                return Ok(await userService.UpdatePassword(id, passwordUpdateParam));
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }catch(UnauthorizedAccessException uae)
            {
                return BadRequest(uae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest(ioe.Message);
            }
        }
    }
}
