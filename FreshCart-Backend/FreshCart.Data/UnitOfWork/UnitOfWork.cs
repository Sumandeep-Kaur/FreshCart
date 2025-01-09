using FreshCart.Data.DBContext;
using FreshCart.Data.Entities;
using FreshCart.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FreshCartContext _context;
        private bool _disposed;

        private IRepository<User> _users;
        private IRepository<Category> _categories;
        private IRepository<Product> _products;
        private IRepository<Cart> _carts;
        private IRepository<CartItem> _cartItems;
        private IRepository<Order> _orders;
        private IRepository<OrderItem> _orderItems;
        private IRepository<Review> _reviews;

        public UnitOfWork(FreshCartContext context)
        {
            _context = context;
        }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
        public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
        public IRepository<Cart> Carts => _carts ??= new Repository<Cart>(_context);
        public IRepository<CartItem> CartItems => _cartItems ??= new Repository<CartItem>(_context);
        public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);
        public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);
        public IRepository<Review> Reviews => _reviews ??= new Repository<Review>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
