using Microsoft.EntityFrameworkCore;

namespace ChiaPool.Models
{
    public class MinerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Miner> Miners { get; set; }

        public MinerContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                .ValueGeneratedOnAdd();

                b.Property(x => x.Name);
                b.Property(x => x.PasswordHash);

                b.HasMany(x => x.Miners)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId);

                b.Ignore(x => x.MinerState);

                b.ToTable("Users");
            });

            modelBuilder.Entity<Miner>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id);

                b.Property(x => x.Name);
                b.HasIndex(x => x.Name)
                .IsUnique();

                b.Property(x => x.Token);
                b.HasIndex(x => x.Token)
                .IsUnique();

                b.Property(x => x.LastAddress);
                b.Property(x => x.LastPlotCount);

                b.Property(x => x.PlotMinutes);
                b.Property(x => x.NextIncrement);

                b.Property(x => x.OwnerId);

                b.ToTable("Miners");
            });
        }
    }
}
