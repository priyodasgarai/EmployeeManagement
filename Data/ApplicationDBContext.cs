using Employee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Employee.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }

      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            /**********User Role*************************/
            List<IdentityRole> roles =
                new List<IdentityRole> {
                    new IdentityRole {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole {
                        Name = "User",
                        NormalizedName = "USER" },
                    new IdentityRole {
                        Name = "Store",
                        NormalizedName = "STORE"
                    }
                };
            builder.Entity<IdentityRole>().HasData(roles);
            /**********User Role*************************/
            List<IdentityUser> AppUser =
               new List<IdentityUser> {
                    new IdentityUser {
                      //  name = "admin",
                        Email = "admin@gmail.com",
                        PasswordHash = "123456",
                        UserName = "admin"
                    }
               };
            builder.Entity<IdentityUser>().HasData(AppUser);
        }



    }
}