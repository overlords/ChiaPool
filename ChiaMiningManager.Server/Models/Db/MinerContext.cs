using Microsoft.EntityFrameworkCore;

namespace ChiaPool.Models
{
    public class MinerContext : DbContext
    {
        public DbSet<Miner> Miners { get; set; }

        public MinerContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Miner>(b =>
            {
                b.Property(x => x.Id);
                b.HasKey(x => x.Id);

                b.Property(x => x.Name);
                b.HasIndex(x => x.Name)
                .IsUnique();

                b.Property(x => x.Token);
                b.HasIndex(x => x.Token)
                .IsUnique();

                b.Property(x => x.Address);
                b.Property(x => x.PlotMinutes);
                b.Property(x => x.NextIncrement);
            });
        }
    }
}
