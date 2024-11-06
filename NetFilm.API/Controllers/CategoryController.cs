using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.Interfaces;

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
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(ChangeCategoryDto changeCategoryDto)
        {
            var category = await _categoryService.AddAsync(changeCategoryDto);
            if (category == null)
            {
                return BadRequest();
            }
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, ChangeCategoryDto changeCategoryDto)
        {
            var category = await _categoryService.UpdateAsync(id, changeCategoryDto);
            if (category == null)
            {
                return BadRequest();
            }
            return Ok(category);
        }
    }
}
