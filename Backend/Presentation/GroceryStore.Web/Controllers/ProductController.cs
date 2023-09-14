using GroceryStore.Data.EntityModels;
using GroceryStore.Services.Repositories.ProductRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("AllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAll();
            return Ok(products);
        }

        [HttpGet("Product/id")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products = await _productRepository.GetAll();
            var product = products.Where(p => p.Id == id).FirstOrDefault();
            return Ok(product);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _productRepository.Add(product);
            return Ok(new { Status = "Success", Message = "Product added successfully!" });
        }

        [HttpPut("Update/id")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            var entity = await _productRepository.Find(id);
            if(entity is not null)
            {
                await _productRepository.Edit(entity, product);
                return Ok(new { Status = "Success", Message = "Product updated successfully!" });
            }
            return BadRequest(new { Status = "Failure", Message = "Product not found in database" });
        }

        [HttpDelete("Delete/id")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var entity = _productRepository.Find(id);
            if (entity is not null)
            {
                await _productRepository.Delete(id);
                return Ok(new { Status = "Success", Message = "Product deleted successfully!" });
            }
            return BadRequest(new { Status = "Failure", Message = "Product not found in database" });
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            review.ReviewDate = DateTime.Now;
            await _productRepository.AddReview(review);
            return Ok(new { Status = "Success", Message = "Review added successfully!" });
        }

        [HttpGet("Get/isCategory/name")]
        public async Task<IActionResult> Search(string name, bool isCategory)
        {
            var products = await _productRepository.Search(name, isCategory);
            return Ok(products);
        }
    }
}
