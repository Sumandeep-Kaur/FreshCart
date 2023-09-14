using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.ProductRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task Edit(Product entity, Product product);
        Task AddReview(Review review);
        Task<IEnumerable<Product>> Search(string name, bool isCategory);
    }
}
