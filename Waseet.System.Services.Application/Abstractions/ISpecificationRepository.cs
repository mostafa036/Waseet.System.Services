using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.IServices;
using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Application.Abstractions
{
    public interface ISpecificationRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec);
        Task<int> GetCountAsync(ISpecifications<T> spec);
    }
}
