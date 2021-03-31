using Microsoft.EntityFrameworkCore;

namespace ChiaMiningManager.Models
{
    public class ConfigurationContext : DbContext
    {
        public DbSet<PlotInfo> Plots { get; set; }

        public ConfigurationContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlotInfo>(b =>
            {
                b.Property(x => x.PublicKey);
                b.HasKey(x => x.PublicKey);

                b.Property(x => x.FileName);
                b.Property(x => x.Minutes);

                b.ToTable("PlotInfos");
            });
        }
    }
}
