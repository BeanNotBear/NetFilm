using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFilm.Application.DTOs.RoleDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;

namespace NetFilm.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IMapper mapper;

        public RoleService(RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<RoleDto> Add(AddRoleRequestDto addRoleRequestDto)
        {
            var roleExists = await roleManager.RoleExistsAsync(addRoleRequestDto.Name.ToUpper());
            if (!roleExists)
            {
                var newRole = new IdentityRole<Guid>(addRoleRequestDto.Name.ToUpper());
                var result = await roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return new RoleDto
                    {
                        Id = newRole.Id,
                        Name = newRole.Name
                    };
                }

                throw new ApplicationException("Role creation failed!");
            }

            throw new ApplicationException($"Role {addRoleRequestDto.Name.ToUpper()} already exists");
        }

        public async Task<List<RoleDto>> GetAll()
        {
            var roles = roleManager.Roles.OrderBy(x => x.Name).ToList();

            return mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetById(Guid id)
        {
            var role = await roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new NotFoundException($"Role with ID {id} was not found");
            }

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<RoleDto> Update(Guid id, UpdateRoleRequest updateRoleRequest)
        {
            var role = await roleManager.FindByIdAsync(id.ToString());
            if(role == null)
            {
                throw new NotFoundException($"Role With ID {id} was not found");
            }

            if(role.Name != updateRoleRequest.Name)
            {
                var existingRole = await roleManager.FindByNameAsync(updateRoleRequest.Name);
                if(existingRole != null && existingRole.Id != role.Id)
                {
                    throw new InvalidOperationException($"Role name '{updateRoleRequest.Name}' is already taken");
                }
            }

            role.Name = updateRoleRequest.Name.ToUpper();

            var result = await roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to update role");
            }

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
