using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.IServices;
using Waseet.System.Services.Domain.Common;
using Waseet.System.Services.Infrastructure.InfrastructureBases;
using Waseet.System.Services.Persistence.Data;

namespace Waseet.System.Services.Infrastructure.Repositories
{
    public class SpecificationRepository<T> : ISpecificationRepository<T> where T : BaseEntity
    {
        private readonly WaseetContext _Context;

        public SpecificationRepository(WaseetContext dbContext)
        {
            _Context = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
            => await ApplaySpecification(spec).ToListAsync();

        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec)
             => await ApplaySpecification(spec).FirstOrDefaultAsync();

        public IQueryable<T> ApplaySpecification(ISpecifications<T> specifications)
             => SpecificationEvaluator<T>.GetQuery(_Context.Set<T>(), specifications);

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
                => await ApplaySpecification(spec).CountAsync();
    }
}
