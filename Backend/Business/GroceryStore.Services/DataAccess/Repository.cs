using GroceryStore.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly GroceryStoreDbContext _dbContext;

        public Repository(GroceryStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Count
        {
            get { return _dbContext.Set<T>().Count(); }
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(object Id)
        {
            T entity = await _dbContext.Set<T>().FindAsync(Id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public async Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<T> Find(object Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> Get(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach(var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> FindEntity(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }


        public async Task Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
