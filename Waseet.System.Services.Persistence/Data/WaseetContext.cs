using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Persistence.Data
{
    public class WaseetContext : DbContext
    {
        public WaseetContext(DbContextOptions<WaseetContext> options)
            : base(options)
        {
        }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductReview> productReviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ServicesProviderReview> ServicesProviderReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WaseetContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
