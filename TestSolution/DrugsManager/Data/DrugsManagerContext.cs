using DrugsManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DrugsManager.Data
{
    public class DrugsManagerContext : DbContext
    {
        public DrugsManagerContext (DbContextOptions<DrugsManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Drug> Drug { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Drug>()
                .HasIndex(b => b.Ndc)
                .IsUnique();
        }
    }
}
