using BadgeCraft_Net.Models;
using Microsoft.EntityFrameworkCore;

namespace BadgeCraft_Net.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Organization> Organizations => Set<Organization>();

        public DbSet<BadgeTemplate> BadgeTemplates { get; set; }

        public DbSet<Badge> Badges { get; set; }

    }
}
