using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrivateEye.Context;
using PrivateEye.Contracts;
using PrivateEye.Identity;
using PrivateEye.Interface.IRespositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrivateEye.Implementation.Repositries
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        protected ApplicationContext _Context;
        public async Task<T> CreateAsync(T entity)
        {
            await _Context.Set<T>().AddAsync(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _Context.Set<T>().AnyAsync(d => d.Id == id);
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _Context.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _Context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _Context.Set<T>().Update(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }
        public void DeleteAsync(T entity)
        {
            _Context.Set<T>().Remove(entity);
            _Context.SaveChanges();
        }
        public IQueryable<T> QueryAsync()
        {
            return _Context.Set<T>().ToList().AsQueryable();
        }

        public IQueryable<T> QueryAsync(Expression<Func<T, bool>> expression)
        {
            return _Context.Set<T>().Where(expression).ToList().AsQueryable();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<T>> GetAsync(IList<int> ids)
        {
            return await _Context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
        {
            return await _Context.Set<T>().AnyAsync(expression);
        }
    }
}
