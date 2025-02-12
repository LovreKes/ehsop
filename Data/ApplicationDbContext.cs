using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using eshop.Models; // ✅ Provjeri da namespace odgovara novom nazivu projekta

namespace eshop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }  // ✅ Provjeri da postoji
        public DbSet<CartItem> CartItems { get; set; }  // ✅ Provjeri da postoji
    }
}
