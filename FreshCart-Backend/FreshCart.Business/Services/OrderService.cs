using AutoMapper;
using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using FreshCart.Data.Entities;
using FreshCart.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderFromCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.Query()
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart?.Items == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Cart is empty");
            }

            // Validate stock and calculate total
            decimal totalAmount = 0;
            foreach (var item in cart.Items)
            {
                if (item.Product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for product: {item.Product.Name}");
                }

                decimal itemPrice = item.Product.DiscountPercentage.HasValue
                    ? item.Product.Price * (1 - item.Product.DiscountPercentage.Value / 100)
                    : item.Product.Price;

                totalAmount += itemPrice * item.Quantity;
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                Status = "Pending",
                TotalAmount = totalAmount,
                OrderDate = DateTime.UtcNow
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // Create order items and update stock
            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    DiscountPercentage = cartItem.Product.DiscountPercentage,
                    UnitPrice = cartItem.Product.DiscountPercentage.HasValue
                        ? cartItem.Product.Price * (1 - cartItem.Product.DiscountPercentage.Value / 100)
                        : cartItem.Product.Price
                };

                await _unitOfWork.OrderItems.AddAsync(orderItem);

                // Update product stock
                cartItem.Product.Stock -= cartItem.Quantity;
                await _unitOfWork.Products.UpdateAsync(cartItem.Product);
            }

            // Clear the cart
            foreach (var item in cart.Items)
            {
                await _unitOfWork.CartItems.DeleteAsync(item.Id);
            }

            await _unitOfWork.SaveChangesAsync();

            return await GetOrderByIdAsync(order.Id, userId);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.Query()
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, int userId)
        {
            var order = await _unitOfWork.Orders.Query()
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _unitOfWork.Orders.Query()
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            var validStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            if (!validStatuses.Contains(status))
            {
                throw new InvalidOperationException("Invalid order status");
            }

            order.Status = status;
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }
    }
}
