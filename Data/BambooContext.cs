using Bamboo.Models;
using Microsoft.EntityFrameworkCore;

namespace Bamboo.Data
{
    public class BambooContext : DbContext
    {
        public BambooContext(DbContextOptions<BambooContext> opts) : base(opts)
        {
            
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
