using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Persistence.Data.Configurations
{

    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> Entity)
        {
            Entity.ToTable("Products");

            Entity.HasKey(p => p.Id);

            Entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            Entity.Property(p => p.OldPrice).HasColumnType("decimal(18,2)");


            Entity.Property(p=>p.Description).IsRequired();

            Entity.HasOne(p => p.Category).WithMany(p => p.Products).HasForeignKey(p => p.CategoryId);

            Entity.HasMany(p => p.ProductReviews).WithOne(p => p.Product).HasForeignKey(p => p.ProductId);

            Entity.Property(p => p.ServiceProviderEmail).IsRequired();

        }
    }
}
