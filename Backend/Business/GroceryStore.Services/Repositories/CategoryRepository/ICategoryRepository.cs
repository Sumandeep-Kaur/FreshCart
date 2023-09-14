using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using System;

namespace GroceryStore.Services.Repositories.CategoryRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}
