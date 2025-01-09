using FreshCart.Data.Entities;
using FreshCart.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Category> Categories { get; }
        IRepository<Product> Products { get; }
        IRepository<Cart> Carts { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<Review> Reviews { get; }
        Task<int> SaveChangesAsync();
    }
}
