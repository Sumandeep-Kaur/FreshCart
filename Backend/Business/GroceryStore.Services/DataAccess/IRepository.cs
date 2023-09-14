using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.DataAccess
{
    public interface IRepository<T> where T : class
    {
        int Count { get; }
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Get(params Expression<Func<T, object>>[] includes);
        Task<T> Find(object Id);
        Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate);
        Task<T> FindEntity(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task Delete(object Id);
        Task<int> SaveAsync();
    }
}
