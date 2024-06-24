using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KhumaloCraft.Models;

namespace KhumaloCraft.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KhumaloCraft.Models.ProductInformation> ProductInformation { get; set; } = default!;
        public DbSet<KhumaloCraft.Models.Cart> Cart { get; set; } = default!;
    }
}
