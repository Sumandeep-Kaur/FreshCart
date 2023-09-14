using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.CartRepository
{
    public interface ICartRepository : IRepository<CartItem>
    {
        Task AddCart(CartItem entity, string userid);
        Task<IEnumerable<CartItem>> GetUserCart(string userId);
        Task<Cart> FindEntity(Expression<Func<Cart, bool>> predicate);
        Task Edit(CartItem item);
    }
}
