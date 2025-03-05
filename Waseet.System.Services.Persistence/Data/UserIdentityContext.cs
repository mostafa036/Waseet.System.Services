using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Identity;

namespace Waseet.System.Services.Persistence.Data
{
    public class UserIdentityContext : IdentityDbContext<User>
    {
        public UserIdentityContext(DbContextOptions<UserIdentityContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.ApplyConfiguration(new AddressConfiguration());
        //    builder.ApplyConfiguration(new CountryConfiguration());
        //    builder.ApplyConfiguration(new ProfilePhotoesConfiguration());
        //    builder.ApplyConfiguration(new UserConfiguration());
        //}