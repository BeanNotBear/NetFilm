using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<PagedResult<UserDto>> GetUserPagedResult(UserQueryParams queryParams)
        {
            // Validate parameters
            queryParams.Validate();

            // Start with all users as IQueryable
            IQueryable<User> query = userManager.Users;

            // Apply search filters if searchTerm is provided
            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var searchTerm = queryParams.SearchTerm.Trim().ToLower();
                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.UserName.ToLower().Contains(searchTerm)
                );
            }

            // Apply sorting
            query = queryParams.SortBy?.ToLower() switch
            {
                "firstname" => queryParams.Ascending
                    ? query.OrderBy(u => u.FirstName)
                    : query.OrderByDescending(u => u.FirstName),
                "lastname" => queryParams.Ascending
                    ? query.OrderBy(u => u.LastName)
                    : query.OrderByDescending(u => u.LastName),
                "email" => queryParams.Ascending
                    ? query.OrderBy(u => u.Email)
                    : query.OrderByDescending(u => u.Email),
                "username" => queryParams.Ascending
                    ? query.OrderBy(u => u.UserName)
                    : query.OrderByDescending(u => u.UserName),
                "dateofbirth" => queryParams.Ascending
                    ? query.OrderBy(u => u.DateOfBirth)
                    : query.OrderByDescending(u => u.DateOfBirth),
                _ => query.OrderBy(u => u.UserName) // default sorting
            };

            // Get total count before pagination
            var totalItems = await query.CountAsync();

            // Apply pagination
            var users = await query
                .Skip((queryParams.PageIndex - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            // Get user roles and map to DTOs
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = mapper.Map<UserDto>(user);
                IList<string> list = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDto.Roles = list.ToArray();
                userDtos.Add(userDto);
            }

            return new PagedResult<UserDto>(userDtos, totalItems, queryParams.PageIndex, queryParams.PageSize);
        }

        public async Task<UserDto> Update(Guid id, UpdateUserRequestDto updateUserRequestDto)
        {
            // Find the user by id
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found");
            }

            // Check if username is already taken by another user
            var existingUserByUsername = await userManager.FindByNameAsync(updateUserRequestDto.UserName);
            if (existingUserByUsername != null && existingUserByUsername.Id != id)
            {
                throw new InvalidOperationException($"Username '{updateUserRequestDto.UserName}' is already taken");
            }

            // Check if email is already taken by another user
            var existingUserByEmail = await userManager.FindByEmailAsync(updateUserRequestDto.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != id)
            {
                throw new InvalidOperationException($"Email '{updateUserRequestDto.Email}' is already registered");
            }

            // Update user properties
            user.FirstName = updateUserRequestDto.FirstName;
            user.LastName = updateUserRequestDto.LastName;
            user.DateOfBirth = updateUserRequestDto.DateOfBirth;
            user.UserName = updateUserRequestDto.UserName;
            user.Email = updateUserRequestDto.Email;
            user.PhoneNumber = updateUserRequestDto.PhoneNumber;
            user.NormalizedEmail = userManager.NormalizeEmail(updateUserRequestDto.Email);
            user.NormalizedUserName = userManager.NormalizeName(updateUserRequestDto.UserName);

            // Update user in the database
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Handle role updates if provided
            if (updateUserRequestDto.Roles != null && updateUserRequestDto.Roles.Any())
            {
                // Get current user roles
                var currentRoles = await userManager.GetRolesAsync(user);

                // Remove existing roles
                if (currentRoles.Any())
                {
                    await userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Validate that all requested roles exist
                foreach (var role in updateUserRequestDto.Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        throw new InvalidOperationException($"Role {role} does not exist");
                    }
                }

                // Add new roles
                result = await userManager.AddToRolesAsync(user, updateUserRequestDto.Roles);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to update user roles: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Refresh user from database to ensure we have the latest data
            user = await userManager.FindByIdAsync(id.ToString());

            var userDto = mapper.Map<UserDto>(user);
            userDto.Roles = (await userManager.GetRolesAsync(user)).ToArray();

            return userDto;
        }

        public async Task<UserDto> UpdatePassword(Guid id, PasswordUpdateParam passwordUpdateParam)
        {
            // Find the user by id
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            // Check if the new password and confirmation password match
            if (passwordUpdateParam.NewPassword != passwordUpdateParam.ConfirmNewPassword)
            {
                throw new ArgumentException("The new password and confirmation password do not match.");
            }

            // Verify the old password
            var passwordCheck = await userManager.CheckPasswordAsync(user, passwordUpdateParam.OldPassword);
            if (!passwordCheck)
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }

            // Update the password
            var result = await userManager.ChangePasswordAsync(user, passwordUpdateParam.OldPassword, passwordUpdateParam.NewPassword);
            if (!result.Succeeded)
            {
                // Collect all error messages
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update password: {errors}");
            }

            // Map the updated user to a UserDto
            var userDto = mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}
