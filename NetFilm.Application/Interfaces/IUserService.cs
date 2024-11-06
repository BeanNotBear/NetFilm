﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.DTOs.UserDTOs;

namespace NetFilm.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();
        Task<UserDto> GetById(Guid id);
        Task<UserDto> Add(AddUserRequestDto addUserRequestDto);
        Task<UserDto> Update(UpdateUserRequestDto updateUserRequestDto);
    }
}