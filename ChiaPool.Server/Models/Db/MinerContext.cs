using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Models
{
    public class MinerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Miner> Miners { get; set; }
        public DbSet<Plotter> Plotters { get; set; }
        public DbSet<PlotTransfer> PlotTranfers { get; set; }

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
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();

                b.Property(x => x.Name);
                b.HasIndex(x => x.Name)
                .IsUnique();

                b.Property(x => x.PasswordHash);

                b.Property(x => x.PlotMinutes)
                .IsConcurrencyToken();

                b.HasMany(x => x.Miners)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId);

                b.HasMany(x => x.Plotters)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId);

                b.ToTable("Users");
            });

            modelBuilder.Entity<Miner>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();

                b.Property(x => x.Name);
                b.HasIndex(x => x.Name)
                .IsUnique();

                b.Property(x => x.Token);
                b.HasIndex(x => x.Token)
                .IsUnique();

                b.Property(x => x.Earnings)
                .IsConcurrencyToken();

                b.Property(x => x.OwnerId);

                b.ToTable("Miners");
            });

            modelBuilder.Entity<Plotter>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();

                b.Property(x => x.Name);
                b.HasIndex(x => x.Name)
                .IsUnique();

                b.Property(x => x.Token);
                b.HasIndex(X => X.Token)
                .IsUnique();

                b.Property(x => x.Earnings)
                .IsConcurrencyToken();

                b.Property(x => x.OwnerId);

                b.ToTable("Plotters");
            });

            modelBuilder.Entity<PlotTransfer>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();

                b.Property(x => x.PlotterId);
                b.Property(x => x.MinerId);

                b.Property(x => x.Cost);

                b.Property(x => x.DownloadAddress);
                b.Property(x => x.Deadline);

                b.ToTable("PlotTransfers");
            });
        }

        public async Task SaveChangesConcurrentAsync()
        {
            bool saveSuccessful = false;
            while (!saveSuccessful)
            {
                try
                {
                    await SaveChangesAsync();
                    saveSuccessful = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    foreach (var entry in e.Entries)
                    {
                        var databaseValues = await entry.GetDatabaseValuesAsync();
                        var originalValues = entry.OriginalValues;
                        var proposedValues = entry.CurrentValues;

                        foreach (var property in proposedValues.Properties.Where(x => x.IsConcurrencyToken))
                        {
                            long valueChange = (long)proposedValues[property] - (long)originalValues[property];
                            proposedValues[property] = (long)databaseValues[property] + valueChange;
                        }

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
            }
        }
    }
}
