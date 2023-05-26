using Bamboo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bamboo.Data
{
    public class BambooContext : IdentityDbContext<User>
    {
        public BambooContext(DbContextOptions<BambooContext> opts) : base(opts)
        {
            
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Log> Logs { get; set; }

    }
}
