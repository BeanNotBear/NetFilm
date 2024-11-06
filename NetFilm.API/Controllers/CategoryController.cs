using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;

namespace NetFilm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories() 
        {
            var categories =  await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(ChangeCategoryDto changeCategoryDto)
        {
            var category = await _categoryService.AddAsync(changeCategoryDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, ChangeCategoryDto changeCategoryDto)
        {
            var category = await _categoryService.UpdateAsync(id, changeCategoryDto);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            await _categoryService.HardDelete(id);
            return NoContent();
        }
    }
}
