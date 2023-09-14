using GroceryStore.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task AddOrder(List<CartItem> items, string userId);
        Task<IEnumerable<OrderItem>> GetUserOrders(string userId);
        Task<IEnumerable<Product>> GetTopFive(int month);
    }
}
