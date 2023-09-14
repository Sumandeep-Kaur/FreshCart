using GroceryStore.Data.EntityModels;
using GroceryStore.Services.Repositories.CartRepository;
using GroceryStore.Services.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("GetCart/id")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var cart = await _cartRepository.GetUserCart(userId);
            return Ok(cart);
        }

        [HttpPost("add/id")]
        public async Task<IActionResult> AddToCart([FromBody] Product product, string userId, int quantity = 1)
        {
            CartItem item = new CartItem
            {
                ProductId = product.Id,
                Quantity = quantity
            };
            await _cartRepository.AddCart(item, userId);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCart([FromBody] List<CartItem> items)
        {
            foreach(var item in items)
            {
                await _cartRepository.Edit(item);
            }
            return Ok();
        }

        [HttpDelete("Delete/id")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var entity = _cartRepository.Find(id);
            if (entity is not null)
            {
                await _cartRepository.Delete(entity);
                return Ok(new { Status = "Success", Message = "Item deleted successfully!" });
            }
            return BadRequest(new { Status = "Failure", Message = "Item not found in database" });
        }
    }
}
