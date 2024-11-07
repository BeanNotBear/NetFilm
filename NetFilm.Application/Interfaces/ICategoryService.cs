using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.DTOs.MovieCategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(Guid id);
        Task<CategoryDto> Add(ChangeCategoryDto changeCategoryDto);
        Task<CategoryDto> Update(Guid id, ChangeCategoryDto changeCategoryDto);
        Task<bool> HardDelete(Guid id);

    }
}
