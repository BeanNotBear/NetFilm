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
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(Guid id);
        Task<CategoryDto> GetByNameAsync(string name);
        Task<CategoryDto> AddAsync(ChangeCategoryDto changeCategoryDto);
        Task<CategoryDto> UpdateAsync(Guid id, ChangeCategoryDto changeCategoryDto);
        Task<CategoryDto> DeleteAsync(Guid id);

    }
}
