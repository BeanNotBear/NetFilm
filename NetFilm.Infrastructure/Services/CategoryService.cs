using AutoMapper;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.DTOs.MovieCategoryDtos;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> AddAsync(ChangeCategoryDto changeCategoryDto)
        {
            var category = _mapper.Map<Category>(changeCategoryDto);
            category.Id = new Guid();
            return _mapper.Map<CategoryDto>(await _categoryRepository.AddAsync(category));
        }

        public Task<CategoryDto> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CategoryDto>>(await GetAllAsync());
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            return _mapper.Map<CategoryDto>( await GetByIdAsync(id));
        }

        public async Task<CategoryDto> GetByNameAsync(string name)
        {
            return _mapper.Map<CategoryDto>(await GetByNameAsync(name));
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, ChangeCategoryDto changeCategoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }
            _mapper.Map(changeCategoryDto,category);
            return _mapper.Map<CategoryDto>(await _categoryRepository.UpdateAsync(category));
        }
    }
}
