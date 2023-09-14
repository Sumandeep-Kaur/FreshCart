using GroceryStore.Data.Data;
using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepository<Product> _repository;
        private readonly IRepository<Review> _reviewRepository;
        public ProductRepository(IRepository<Product> repository, IRepository<Review> reviewRepository)
        {
            _repository = repository;
            _reviewRepository = reviewRepository;
        }

        public int Count => _repository.Count;

        public async Task Add(Product entity)
        {
            await _repository.Add(entity);
            await _repository.SaveAsync();
        }

        public async Task Delete(Product entity)
        {
            await _repository.Delete(entity);
        }

        public async Task Delete(object Id)
        {
            await _repository.Delete(Id);
            await _repository.SaveAsync();
        }

        public Task<IEnumerable<Product>> Filter(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> Find(object Id)
        {
            return await _repository.Find(Id);
        }

        public async Task<Product> FindEntity(Expression<Func<Product, bool>> predicate)
        {
            return await _repository.FindEntity(predicate);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            Expression<Func<Product, object>>[] include = new Expression<Func<Product, object>>[]
            {
                p => p.Category,
                p => p.Reviews
            };
            return await this.Get(include);
        }

        public async Task<IEnumerable<Product>> Get(params Expression<Func<Product, object>>[] includes)
        {
            return await _repository.Get(includes);
        }

        public async Task<int> SaveAsync()
        {
            return await _repository.SaveAsync();
        }

        public async Task Update(Product entity)
        {
            await _repository.Update(entity);
        }

        public async Task Edit(Product product, Product entity)
        {
            //Product product = await _repository.Find(id);
            product.Name = entity.Name;
            product.Description = entity.Description;
            product.CategoryId = entity.CategoryId;
            product.Price = entity.Price;
            product.Discount = entity.Discount;
            product.UnitsInStock = entity.UnitsInStock;
            product.Specs = entity.Specs;
            product.ImageUrl = entity.ImageUrl;
            await _repository.Update(product);
            await _repository.SaveAsync();
        }

        public async Task AddReview(Review review)
        {
            await _reviewRepository.Add(review);
            await _reviewRepository.SaveAsync();
            Product product = await _repository.FindEntity(p => p.Id == review.ProductId);
            product.Reviews.Add(review);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<Product>> Search(string name, bool isCategory)
        {
            Expression<Func<Product, object>> include = p => p.Category;
            var products = await this.Get(include);
            if(isCategory)
            {
                return products.Where(p => p.Category.CategoryName == name).ToList();
            }
            return products.Where(p => p.Name.Contains(name)).ToList();
        }
    }
}
