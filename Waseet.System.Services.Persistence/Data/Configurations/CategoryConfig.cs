using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Persistence.Data.Configurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> Entity)
        {
            Entity.ToTable("Category");

            Entity.HasKey(c => c.Id);

            Entity.Property(c => c.CategoryName).IsRequired();

            Entity.HasMany(c => c.Products).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId);
        }
    }
}
