using Microsoft.EntityFrameworkCore;
using SpecTacular.Models;


namespace SpecTacular.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // Ensure this matches your model
        public DbSet<Product> Products { get; set; } // This represents the Product table

        public DbSet<Promotion> Promotions { get; set; }
    }
}
