using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.RoleDTOs;
using NetFilm.Domain.Common;

namespace NetFilm.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAll();
        Task<PagedResult<RoleDto>> GetAllPagination();
        Task<RoleDto> GetById(Guid id);
        Task<RoleDto> Add(AddRoleRequestDto addRoleRequestDto);
        Task<RoleDto> Update(Guid id, UpdateRoleRequest updateRoleRequest);
    }
}
