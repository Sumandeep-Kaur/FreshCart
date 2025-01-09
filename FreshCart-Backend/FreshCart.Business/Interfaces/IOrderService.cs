using FreshCart.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderFromCartAsync(int userId);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<OrderDto> GetOrderByIdAsync(int orderId, int userId);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status);
    }
}
