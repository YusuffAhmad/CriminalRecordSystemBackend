using PrivateEye.Contracts;
using PrivateEye.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrivateEye.Interface.IRespositries
{
   public interface IBaseRepository<T>
   {
        IQueryable<T> QueryAsync();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> GetAsync(int id);
        Task<IList<T>> GetAsync(IList<int> ids);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        void DeleteAsync(T entity);
        Task<bool> ExistsAsync(int id);
        IQueryable<T> QueryAsync(Expression<Func<T, bool>> expression);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
    }
}
