using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var password = "admin";
            var sha256 = new SHA256Managed();
            var passwordHash = Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

            Role roleUser = new Role { Id = 1, Name = "user" };
            Role roleAdmin = new Role { Id = 2, Name = "admin" };

            User userAdmin = new User
            {
                Id = 1,
                Email = "admin@gmail.com",
                Password = passwordHash,
                RoleId = 2
            };

            modelBuilder.Entity<Role>().HasData(roleAdmin, roleUser);
            modelBuilder.Entity<User>().HasData(userAdmin);

            base.OnModelCreating(modelBuilder);
        }
    }
}
