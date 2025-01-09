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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<OrderDto>> CreateOrder()
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _orderService.CreateOrderFromCartAsync(userId);
                return CreatedAtAction(nameof(GetOrder), new { orderId = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "An error occurred while creating the order");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            try
            {
                var userId = GetCurrentUserId();
                var orders = await _orderService.GetUserOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                return StatusCode(500, "An error occurred while retrieving orders");
            }
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int orderId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _orderService.GetOrderByIdAsync(orderId, userId);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order");
                return StatusCode(500, "An error occurred while retrieving the order");
            }
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(orderId, status);
                return Ok(order);
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
                _logger.LogError(ex, "Error updating order status");
                return StatusCode(500, "An error occurred while updating the order status");
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
