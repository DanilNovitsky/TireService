using Microsoft.EntityFrameworkCore;
using TireServiceWeb.Models;

namespace TireServiceWeb.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<TireShop> TireShops { get; set; }
        public DbSet<Tire> Tires { get; set; }
    }
}
