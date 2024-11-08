using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFilm.Application.DTOs.AuthDTOs;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;

namespace NetFilm.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly ITokenRepository tokenRepository;

        public AuthService(UserManager<User> userManager,
            IMapper mapper,
            RoleManager<IdentityRole<Guid>> roleManager,
            ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.tokenRepository = tokenRepository;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByNameAsync(loginRequestDto.UserName);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.PassWord);

                if (checkPasswordResult)
                {
                    // Get Role for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if(roles != null)
                    {
                        // Create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return response;
                    }
                }
            }
            throw new NotAuthorizationException("Login Fail!");
        }

        public async Task<UserDto> Register(RegisterRequestDto registerRequestDto)
        {
            // Check if user exists
            var existingUser = await userManager.FindByEmailAsync(registerRequestDto.Email);
            if (existingUser != null)
            {
                throw new ApplicationException("User with this email already exists");
            }

            // Map and create user
            var user = mapper.Map<User>(registerRequestDto);
            var identityResult = await userManager.CreateAsync(user, registerRequestDto.PassWord);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to create user: {errors}");
            }

            identityResult = await userManager.AddToRoleAsync(user, "User");
            if (!identityResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to assign role: {errors}");
            }

            // Map to UserDto
            var userDto = mapper.Map<UserDto>(user);

            // Get and assign roles
            var userRoles = await userManager.GetRolesAsync(user);
            userDto.Roles = userRoles.ToArray();

            return userDto;
        }
    }
}
