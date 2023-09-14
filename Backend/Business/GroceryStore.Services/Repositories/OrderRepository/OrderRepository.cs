using GroceryStore.Data.Data;
using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly GroceryStoreDbContext _dbContext;
        public OrderRepository(IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository,
            IRepository<Cart> cartRepository, IRepository<CartItem> cartItemRepository, IRepository<Product> productRepository, GroceryStoreDbContext dbContext)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _dbContext = dbContext;
        }
        public async Task AddOrder(List<CartItem> items, string userId)
        {
            Order orderEntity = await _orderRepository.FindEntity(c => c.UserId == userId);
            if (orderEntity is null)
            {
                orderEntity = new Order
                {
                    UserId = userId
                };
                await _orderRepository.Add(orderEntity);
            }
            await _orderRepository.SaveAsync();

            foreach(var item in items)
            {  
                OrderItem order = new OrderItem
                {
                    ProductId = item.ProductId,
                    OrderId = orderEntity.Id,
                    Quantity = item.Quantity,
                    OrderDate = DateTime.Now,
                    TotalPrice = item.Quantity * (item.Product.Price - item.Product.Discount) 
                };
                await _orderItemRepository.Add(order);
                await _orderItemRepository.SaveAsync();
                orderEntity.OrderItems.Add(order);

                Product product = await _productRepository.FindEntity(p => p.Id == item.ProductId);
                product.UnitsInStock -= item.Quantity;
                await _productRepository.SaveAsync();

                await _cartItemRepository.Delete(item);
                await _cartItemRepository.SaveAsync();
                Cart cart = await _cartRepository.FindEntity(c => c.UserId == userId);
                cart.CartItems.Remove(item);
            }
        }

        public async Task<IEnumerable<OrderItem>> GetUserOrders(string userId)
        {
            var orderEntity = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.Category)
                .Where(o => o.UserId == userId).FirstOrDefaultAsync();
            return orderEntity == null ? new List<OrderItem>() : orderEntity.OrderItems.ToList();
        }

        public async Task<IEnumerable<Product>> GetTopFive(int targetMonth)
        {
            var list = _dbContext.OrderItems
                .AsEnumerable()
                .Where(item => item.OrderDate.Month == targetMonth)
                .GroupBy(item => item.ProductId)
                .ToDictionary(group => group.Key, group => group.Sum(item => item.Quantity))
                .OrderByDescending(group => group.Value)
                .Take(5)
                .ToList();

            IList<Product> products = new List<Product>();
            foreach(var item in list)
            {
                var product = await _dbContext.Products.Include(p => p.Category).Where(p => p.Id == item.Key).FirstOrDefaultAsync();
                product.UnitsInStock = item.Value;
                products.Add(product);
            }

            return products;
        }
    }
}
