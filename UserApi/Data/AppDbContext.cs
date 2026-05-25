using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi.Data
{
    public class AppDbContext : DbContext
    {
        // constructor with DbContextOptions parameters / why dont wee ned a return type ? how does it work
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        // why do we set null ?  => to say to the start value is null and de compiler has to trust us with ! ef fills this automaticly
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
    }
}
