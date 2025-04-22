using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Common;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Application.Abstractions
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> AddAsync(T entity);
        IQueryable<T> Include(Expression<Func<T, object>> include);
        Task DeleteRange(IEnumerable<T> Entities);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
