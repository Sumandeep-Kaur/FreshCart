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
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartDto> GetUserCartAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return await MapCartToDto(cart);
        }

        public async Task<CartDto> AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            if (product.Stock < quantity)
            {
                throw new InvalidOperationException("Not enough stock available");
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _unitOfWork.CartItems.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                await _unitOfWork.CartItems.UpdateAsync(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();
            return await MapCartToDto(cart);
        }

        public async Task<CartDto> UpdateCartItemAsync(int userId, int cartItemId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            if (quantity <= 0)
            {
                await _unitOfWork.CartItems.DeleteAsync(cartItemId);
            }
            else
            {
                var product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);
                if (product.Stock < quantity)
                {
                    throw new InvalidOperationException("Not enough stock available");
                }

                cartItem.Quantity = quantity;
                await _unitOfWork.CartItems.UpdateAsync(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();
            return await MapCartToDto(cart);
        }

        public async Task RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            await _unitOfWork.CartItems.DeleteAsync(cartItemId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);

            foreach (var item in cart.Items.ToList())
            {
                await _unitOfWork.CartItems.DeleteAsync(item.Id);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.Query()
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            return cart;
        }

        private async Task<CartDto> MapCartToDto(Cart cart)
        {
            var cartDto = _mapper.Map<CartDto>(cart);
            cartDto.TotalAmount = cart.Items.Sum(i =>
                i.Product.DiscountPercentage.HasValue
                    ? i.Product.Price * (1 - i.Product.DiscountPercentage.Value / 100) * i.Quantity
                    : i.Product.Price * i.Quantity);
            return cartDto;
        }
    }
}
