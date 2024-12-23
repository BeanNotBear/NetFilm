﻿using AutoMapper;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.MovieCategoryDtos;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
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

        public async Task<CategoryDto> Add(ChangeCategoryDto changeCategoryDto)
        {
            var isExisted = await _categoryRepository.ExistsByNameAsync(changeCategoryDto.Name);
            if (isExisted) 
            {
                throw new ExistedEntityException($"{changeCategoryDto.Name} is already existed!");
            }
            var category = _mapper.Map<Category>(changeCategoryDto);
            category.Id = new Guid();
            return _mapper.Map<CategoryDto>(await _categoryRepository.AddAsync(category));
        }

        public async Task<bool> HardDelete(Guid id)
        {
            var isExisted = await _categoryRepository.ExistsAsync(id);
            if (!isExisted)
            {
                throw new NotFoundException($"Can not category with Id: {id}");
            }
            var isDeleted = await _categoryRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                throw new Exception("Some things went wrong!");
            }
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<CategoryDto>>(await _categoryRepository.GetAllAsync());
        }

        public async Task<CategoryDto> GetById(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Can not found category with Id {id}");
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> Update(Guid id, ChangeCategoryDto changeCategoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Can not found category with Id {id}");
            }
            var isExisted = await _categoryRepository.ExistsByNameAsync(changeCategoryDto.Name);
            if (isExisted)
            {
                throw new ExistedEntityException($"{changeCategoryDto.Name} is already existed!");
            }
            _mapper.Map(changeCategoryDto,category);
            var updateCategory = await _categoryRepository.UpdateAsync(category);
            if (updateCategory == null)
            {
                throw new Exception("Some things went wrong!");
            }
            return _mapper.Map<CategoryDto>(updateCategory);
        }

        public async Task<PagedResult<CategoryDto>> GetCategoryPagedResult(CategoryQueryParams queryParams)
        {
            // Validate
            queryParams.Validate();

            var categories = await _categoryRepository.GetCategoryPagedResultAsync(queryParams.PageSize, queryParams.PageIndex, queryParams.SearchTerm, queryParams.SortBy, queryParams.Ascending);
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            var totalItem = await _categoryRepository.CountAsync();
            return new PagedResult<CategoryDto>(categoriesDto,totalItem,queryParams.PageIndex,queryParams.PageSize);
        }
    }
}
