using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;

namespace NetFilm.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public async Task<UserDto> Add(AddUserRequestDto addUserRequestDto)
        {
            // Validate input
            if (addUserRequestDto == null)
            {
                throw new ArgumentNullException(nameof(addUserRequestDto));
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

        public Task<List<UserDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<UserDto>> GetPagedResult()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Update(UpdateUserRequestDto updateUserRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Update(Guid id, UpdateUserRequestDto updateUserRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
