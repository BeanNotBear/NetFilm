using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Application.DTOs.UserDTOs;
using NetFilm.Domain.Common;

namespace NetFilm.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();
        Task<PagedResult<UserDto>> GetPagedResult();
        Task<UserDto> GetById(Guid id);
        Task<UserDto> Add(AddUserRequestDto addUserRequestDto);
        Task<UserDto> Update(Guid id, UpdateUserRequestDto updateUserRequestDto);
    }
}
