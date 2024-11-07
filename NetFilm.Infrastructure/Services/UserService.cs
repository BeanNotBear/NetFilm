using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;

namespace NetFilm.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public UserService(UserManager<User> userManager, 
            IMapper mapper,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }
        public async Task<UserDto> Add(AddUserRequestDto addUserRequestDto)
        {
            // Validate input
            if (addUserRequestDto == null)
            {
                throw new ArgumentNullException(nameof(addUserRequestDto));
            }

            // Check if all specified roles exist
            if (addUserRequestDto.Roles?.Length > 0)
            {
                foreach (var role in addUserRequestDto.Roles)
                {
                    var roleExists = await roleManager.RoleExistsAsync(role);
                    if (!roleExists)
                    {
                        throw new ApplicationException($"Role '{role}' does not exist.");
                    }
                }
            }

            // Create new user instance using mapper
            var user = mapper.Map<User>(addUserRequestDto);

            // Create user using UserManager
            var result = await userManager.CreateAsync(user, addUserRequestDto.PassWord);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to create user: {errors}");
            }

            // If roles are specified in the request, assign them
            if (addUserRequestDto.Roles?.Length > 0)
            {
                var roleResult = await userManager.AddToRolesAsync(user, addUserRequestDto.Roles);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));

                    // Clean up - delete the user since role assignment failed
                    await userManager.DeleteAsync(user);

                    throw new ApplicationException($"Failed to assign roles: {errors}");
                }
            }

            // Reload user to get all updated properties including roles
            var updatedUser = await userManager.FindByIdAsync(user.Id.ToString());
            var userRoles = await userManager.GetRolesAsync(updatedUser);

            // Map to DTO and set roles
            var userDto = mapper.Map<UserDto>(updatedUser);
            userDto.Roles = userRoles.ToArray();

            return userDto;
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = userManager.Users.ToList();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = mapper.Map<UserDto>(user);
                IList<string> list = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDto.Roles = list.ToArray();
                userDtos.Add(userDto);
            }

            return userDtos;
        }

        public async Task<UserDto> GetById(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found.");
            }

            var userDto = mapper.Map<UserDto>(user);
            IList<string> list = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            userDto.Roles = list.ToArray();

            return userDto;
        }

        public Task<PagedResult<UserDto>> GetUserPagedResult()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Update(Guid id, UpdateUserRequestDto updateUserRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
