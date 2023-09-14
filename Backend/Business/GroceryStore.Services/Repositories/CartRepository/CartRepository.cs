using GroceryStore.Data.Data;
using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using GroceryStore.Services.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly GroceryStoreDbContext _dbContext;
        public CartRepository(IRepository<CartItem> repository, IRepository<Cart> cartRepository, 
             GroceryStoreDbContext dbContext)
        {
            _cartItemRepository = repository;
            _cartRepository = cartRepository;
            _dbContext = dbContext;
        }

        public int Count => throw new NotImplementedException();

        public async Task AddCart(CartItem entity, string userId)
        {
            Cart cart = await _cartRepository.FindEntity(c => c.UserId == userId);
            if(cart is null)
            {
                cart = new Cart
                {
                    UserId = userId
                };
                await _cartRepository.Add(cart);
            }
            await _cartRepository.SaveAsync();

            var cartItem = await _cartItemRepository.FindEntity(c => c.CartId == cart.Id && c.ProductId == entity.ProductId);

            if(cartItem is not null)
            {
                cartItem.Quantity += entity.Quantity;
            } 
            else
            {
                cartItem = new CartItem
                {
                    ProductId = entity.ProductId,
                    CartId = cart.Id,
                    Quantity = entity.Quantity
                };
                await _cartItemRepository.Add(cartItem);
                cart.CartItems.Add(cartItem);
            }

            await _cartItemRepository.SaveAsync();
        }

        public Task Add(CartItem entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(CartItem item)
        {
            await this.Delete(item.Id);
            Cart cart = await _cartRepository.FindEntity(c => c.Id == item.CartId);
            cart.CartItems.Remove(item);
            await _cartRepository.SaveAsync();
        }

        public async Task Delete(object id)
        {
            await _cartItemRepository.Delete(id);
            await _cartItemRepository.SaveAsync();
        }

        public async Task<IEnumerable<CartItem>> GetUserCart(string userId)
        {
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .ThenInclude(c => c.Category)
                .Where(c => c.UserId == userId).FirstOrDefaultAsync();
            return cart == null ? new List<CartItem>() : cart.CartItems.ToList();
        }

        public async Task<CartItem> Find(object Id)
        {
            return await _cartItemRepository.Find(Id);
        }

        public async Task<Cart> FindEntity(Expression<Func<Cart, bool>> predicate)
        {
            return await _cartRepository.FindEntity(predicate);
        }

        public async Task<IEnumerable<CartItem>> Get(params Expression<Func<CartItem, object>>[] includes)
        {
            return await _cartItemRepository.Get(includes);
        }

        public Task<IEnumerable<CartItem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveAsync()
        {
            return await _cartItemRepository.SaveAsync();
        }

        public Task Update(CartItem entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CartItem>> Filter(Expression<Func<CartItem, bool>> predicate)
        {
            return await _cartItemRepository.Filter(predicate);
        }

        public async Task<CartItem> FindEntity(Expression<Func<CartItem, bool>> predicate)
        {
            return await _cartItemRepository.FindEntity(predicate);
        }

        public async Task Edit(CartItem item)
        {
            CartItem cartItem = await this.Find(item.Id);
            cartItem.Quantity = item.Quantity;
            await _cartItemRepository.SaveAsync();
        }
    }
}
