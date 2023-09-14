using GroceryStore.Data.EntityModels;
using GroceryStore.Services.Repositories.OrderRepository;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("GetOrders/id")]
        public async Task<IActionResult> GetOrders(string userId)
        {
            var cart = await _orderRepository.GetUserOrders(userId);
            return Ok(cart);
        }

        [HttpPost("add/id")]
        public async Task<IActionResult> AddOrder([FromBody] List<CartItem> items, string userId)
        {
            await _orderRepository.AddOrder(items, userId);
            return Ok();
        }

        [HttpGet("GetTopFiveOrders/month")]
        public async Task<IActionResult> GetTopFiveOrders(int month)
        {
            var cart = await _orderRepository.GetTopFive(month);
            return Ok(cart);
        }
    }
}
