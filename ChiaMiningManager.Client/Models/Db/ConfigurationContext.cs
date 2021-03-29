using Microsoft.EntityFrameworkCore;

namespace ChiaMiningManager.Models
{
    public class ConfigurationContext : DbContext
    {
        public DbSet<Plot> Plots { get; set; }

        public ConfigurationContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plot>(b =>
            {
                b.Property(x => x.Id);
                b.HasKey(x => x.Id);

                b.Property(x => x.CreatedAt);

                b.Property(x => x.Name);
                b.HasIndex(x => x.Name)
                .IsUnique();

                b.Property(x => x.Minutes);
                b.Property(x => x.Path);
            });
        }
    }
}
