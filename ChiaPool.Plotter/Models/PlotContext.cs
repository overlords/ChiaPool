using Microsoft.EntityFrameworkCore;

namespace ChiaPool.Models
{
    public class PlotContext : DbContext
    {
        public DbSet<StoredPlot> StoredPlots { get; set; }

        public PlotContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoredPlot>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                .ValueGeneratedOnAdd();

                b.Property(x => x.Path);
                b.Property(x => x.Secret);

                b.Property(x => x.Available);
                b.Property(x => x.Deleted);

                b.ToTable("StoredPlots");
            });
        }
    }
}
