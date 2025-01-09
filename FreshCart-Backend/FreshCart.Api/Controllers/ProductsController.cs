using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreshCart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product {ProductId}", id);
                return StatusCode(500, "An error occurred while retrieving the product");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] ProductCreateDto productCreateDto)
        {
            try
            {
                var product = await _productService.CreateAsync(productCreateDto);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "An error occurred while creating the product");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDto productDto)
        {
            try
            {
                var product = await _productService.UpdateAsync(id, productDto);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return StatusCode(500, "An error occurred while updating the product");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return StatusCode(500, "An error occurred while deleting the product");
            }
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }

                var products = await _productService.SearchAsync(searchTerm);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with term {SearchTerm}", searchTerm);
                return StatusCode(500, "An error occurred while searching products");
            }
        }
    }
}

