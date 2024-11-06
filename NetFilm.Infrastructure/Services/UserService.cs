using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Application.Interfaces;

namespace NetFilm.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;

        public UserService(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public Task<UserDto> Add(AddUserRequestDto addUserRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Update(UpdateUserRequestDto updateUserRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
