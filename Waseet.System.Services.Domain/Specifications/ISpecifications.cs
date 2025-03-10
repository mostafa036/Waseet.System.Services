using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Application.IServices
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } // Support for Includes
        public List<Func<IQueryable<T>, IQueryable<T>>> ThenIncludes { get; }  // Support for ThenIncludes
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; }
    }
}
