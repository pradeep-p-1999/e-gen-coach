using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace VShop.Models
{
    // This class provoids relationship and building overrides to customize in entity framework. 
    public class AuthenticationContext : IdentityDbContext
    {
        // Includes Application to mapping.
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Includes Courses to the mapping.
        public DbSet<Course> Courses { get; set; }

        public AuthenticationContext(DbContextOptions options) : base(options)
        {

        }

        // Overriding automated mapping by entity frameword to our oun custome mapping.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Omits the Image file in Entity Database mapping. 
            builder.Entity<Course>().Ignore(lose =>lose.CoverImage);

            // Creating many2many relationship for User and the courses.
            builder.Entity<ApplicationUser>()
                .HasMany(chain => chain.Courses)
                .WithMany(chain => chain.ApplicationUsers);

            // Omits the Image file in Entity Database mapping. 
            builder.Entity<ApplicationUser>().Ignore(lose => lose.Profileimage);

            base.OnModelCreating(builder);
        }

        internal Task Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
