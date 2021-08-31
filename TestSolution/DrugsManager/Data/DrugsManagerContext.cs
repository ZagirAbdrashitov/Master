using Microsoft.EntityFrameworkCore;

namespace DrugsManager.Data
{
    public class DrugsManagerContext : DbContext
    {
        public DrugsManagerContext (DbContextOptions<DrugsManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Drug> Drug { get; set; }
    }
}
