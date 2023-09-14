using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IRepository<Category> _repository;
        public CategoryRepository(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public int Count => throw new NotImplementedException();

        public async Task Add(Category entity)
        {
            await _repository.Add(entity);
        }

        public async Task Delete(Category entity)
        {
            await _repository.Delete(entity);
        }

        public async Task Delete(object Id)
        {
            await _repository.Delete(Id);
        }

        public Task<IEnumerable<Category>> Filter(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> Find(object Id)
        {
            return await _repository.Find(Id);
        }

        public Task<Category> FindEntity(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> Get(params Expression<Func<Category, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _repository.GetAll();
        }

        public Task<IEnumerable<Category>> GetJoinedEntities(params Expression<Func<Category, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveAsync()
        {
            return await _repository.SaveAsync();
        }

        public async Task Update(Category entity)
        {
            await _repository.Update(entity);
        }
    }
}
