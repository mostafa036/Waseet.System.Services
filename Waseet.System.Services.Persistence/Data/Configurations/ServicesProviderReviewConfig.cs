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
    public class ServicesProviderReviewConfig : IEntityTypeConfiguration<ServicesProviderReview>
    {
        public void Configure(EntityTypeBuilder<ServicesProviderReview> Entity)
        {
            Entity.ToTable("ServicesProviderReviews");

            Entity.HasKey(x => x.Id);

            Entity.Property(x => x.Comment).HasMaxLength(500);
            Entity.Property(s => s.ServiceProviderEmail).IsRequired();
        }
    }
}
