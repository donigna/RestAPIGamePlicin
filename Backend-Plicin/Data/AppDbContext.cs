using Microsoft.EntityFrameworkCore;
using Backend_Plicin.Models;

namespace Backend_Plicin.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; } = null!;
    }
}
