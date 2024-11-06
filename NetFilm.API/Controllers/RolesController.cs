using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.RoleDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;

namespace NetFilm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RolesController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddRole([FromBody] AddRoleRequestDto addRoleRequestDto)
        {
            var roleDto = await roleService.Add(addRoleRequestDto);

            return CreatedAtAction(nameof(GetRoleById), new { id = roleDto.Id }, roleDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id)
        {
            try
            {
                var roleDto = await roleService.GetById(id);
                return Ok(roleDto);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid id, [FromBody] UpdateRoleRequest updateRoleRequest)
        {
            try
            {
                var updatedRole = await roleService.Update(id, updateRoleRequest);
                return Ok(updatedRole);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
