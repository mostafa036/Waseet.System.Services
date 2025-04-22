using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Domain.Common;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Persistence.Data;

namespace Waseet.System.Services.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly WaseetContext _context;

        public BaseRepository(WaseetContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _context.Set<T>().Remove(await GetByIdAsync(id));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteRange(IEnumerable<T> Entities)
        {
            _context.Set<T>().RemoveRange(Entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        //public async Task<List<T>> GetAllByIdAsync(int id)
        //{
        //    return await _context.Set<T>().Where(entity => entity.Id == id).ToListAsync();
        //}

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _context.Set<T>().Where(item => item.Id == id).FirstOrDefaultAsync();

            if (result == null) throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.");

            return result;
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync(predicate);

            if (result == null) throw new KeyNotFoundException($"Entity not found.");

            return result;

        }

        public IQueryable<T> Include(Expression<Func<T, object>> include)
            => _context.Set<T>().Include(include);

        public async Task<T> UpdateAsync(T entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(entity.Id);

            if (existingEntity == null)
                throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return existingEntity;
        }
    }
}
