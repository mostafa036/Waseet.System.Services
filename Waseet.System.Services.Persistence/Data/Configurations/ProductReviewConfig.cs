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
    public class ProductReviewConfig : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> Entity)
        {
            Entity.ToTable("ProductReview");

            Entity.HasKey(p => p.Id);

            Entity.Property(p => p.Comment).IsRequired();

            Entity.Property(p => p.Rating).IsRequired();

            Entity.Property(p=> p.ReviewDate).IsRequired();

            Entity.Property(p => p.CustomerEamil).IsRequired();

            Entity.HasOne(p => p.Product).WithMany(p => p.ProductReviews).HasForeignKey(p => p.ProductId);
        }
    }
}
