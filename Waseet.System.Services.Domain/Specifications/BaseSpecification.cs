using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.IServices;
using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public List<Func<IQueryable<T>, IQueryable<T>>> ThenIncludes { get; } = new List<Func<IQueryable<T>, IQueryable<T>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; }

        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<T, bool>> _Criteria)
        {
            Criteria = _Criteria;
        }

        public void ApplyPaging(int skip, int take)
        {
            IsPagingEnabled = true;
            Take = take;
            Skip = skip;
        }

        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

    }
}
