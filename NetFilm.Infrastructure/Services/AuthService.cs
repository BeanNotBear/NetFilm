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
        private readonly IEmailService emailService;

        public AuthService(UserManager<User> userManager,
            IMapper mapper,
            RoleManager<IdentityRole<Guid>> roleManager,
            ITokenRepository tokenRepository,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.tokenRepository = tokenRepository;
            this.emailService = emailService;
        }

        public async Task<bool> EmailVerification(string email, string code)
        {
            if(code == null && email == null)
            {
                throw new NotFoundException("Couldn't find code");
            }

            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                throw new NotFoundException("Couldn't find email");
            }

            var isVerified = await userManager.ConfirmEmailAsync(user, code);

            if (isVerified.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<ResponseForgotPasswordDto> ForgotPassword(RequestForgotPasswordDto requestForgotPasswordDto)
        {
            // Validate user
            var user = await userManager.FindByEmailAsync(requestForgotPasswordDto.Email);
            if(user == null)
            {
                throw new NotFoundException("Can't found email");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            if(string.IsNullOrEmpty(token))
            {
                throw new NotFoundException("Can't found token");
            }

            var callbackUrl = $"https://localhost:9999/resetpassword?token={token}&email={user.Email}";
            //send email

            return new ResponseForgotPasswordDto
            {
                Token = token,
                Email = user.Email,
            };
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByNameAsync(loginRequestDto.UserName);

            if (user != null)
            {
                if(!await userManager.IsEmailConfirmedAsync(user))
                {
                    throw new NotAuthorizationException("Email is not confirmed");
                }

                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.PassWord);

                if (checkPasswordResult)
                {
                    // Get Role for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
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

            // Add role
            var identityResultRole = await userManager.AddToRoleAsync(user, "User");
            if (!identityResultRole.Succeeded)
            {
                await userManager.DeleteAsync(user);
                var errors = string.Join(", ", identityResultRole.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to assign role: {errors}");
            }

            if (identityResult.Succeeded)
            {
                // Require email confirmation
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                // Email functionality to send the code to user
                emailService.SendEmail(user.Email,"OTP",code);
            }

            // Map to UserDto
            var userDto = mapper.Map<UserDto>(user);

            // Get and assign roles
            var userRoles = await userManager.GetRolesAsync(user);
            userDto.Roles = userRoles.ToArray();

            return userDto;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (user == null)
            {
                throw new NotFoundException("Can't found email");
            }

            if (!resetPasswordRequestDto.Password.Equals(resetPasswordRequestDto.ConfirmPassword))
            {
                throw new ApplicationException("Password and Confirm Password doesn't match");
            }

            var result = await userManager.ResetPasswordAsync(user, resetPasswordRequestDto.Token, resetPasswordRequestDto.Password);
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
