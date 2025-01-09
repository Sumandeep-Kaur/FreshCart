using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreshCart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, "An error occurred while retrieving categories");
            }
        }

        [HttpGet("{id}/products")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryId(int id)
        {
            try
            {
                var products = await _categoryService.GetProductsByCategoryIdAsync(id);
                if (products == null || !products.Any())
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products for category {CategoryId}", id);
                return StatusCode(500, "An error occurred while retrieving the products for this category");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromForm] CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var category = await _categoryService.CreateAsync(categoryCreateDto);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(500, "An error occurred while creating the category");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryCreateDto categoryDto)
        {
            try
            {
                var category = await _categoryService.UpdateAsync(id, categoryDto);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category {CategoryId}", id);
                return StatusCode(500, "An error occurred while updating the category");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category {CategoryId}", id);
                return StatusCode(500, "An error occurred while deleting the category");
            }
        }

    }
}
