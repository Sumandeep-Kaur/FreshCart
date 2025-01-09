using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreshCart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                var cart = await _cartService.GetUserCartAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart");
                return StatusCode(500, "An error occurred while retrieving the cart");
            }
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddToCart(int productId, int quantity)
        {
            try
            {
                var userId = GetCurrentUserId();
                var cart = await _cartService.AddToCartAsync(userId, productId, quantity);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return StatusCode(500, "An error occurred while adding the item to cart");
            }
        }

        [HttpPut("items/{cartItemId}")]
        public async Task<ActionResult<CartDto>> UpdateCartItem(int cartItemId, int quantity)
        {
            try
            {
                var userId = GetCurrentUserId();
                var cart = await _cartService.UpdateCartItemAsync(userId, cartItemId, quantity);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return StatusCode(500, "An error occurred while updating the cart item");
            }
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _cartService.RemoveFromCartAsync(userId, cartItemId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return StatusCode(500, "An error occurred while removing the item from cart");
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                await _cartService.ClearCartAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return StatusCode(500, "An error occurred while clearing the cart");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user token");
            }
            return userId;
        }
    }

}
