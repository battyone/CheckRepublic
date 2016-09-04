using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckContext : DbContext
    {
        public CheckContext(DbContextOptions<CheckContext> options) : base(options)
        {
        }

        public DbSet<Check> Checks { get; set; }
        public DbSet<CheckBatch> CheckBatches { get; set; }
        public DbSet<CheckResult> CheckResults { get; set; }
        public DbSet<Heartbeat> Heartbeats { get; set; }
        public DbSet<Heart> Hearts { get; set; }
        public DbSet<HeartGroup> HeartGroups { get; set; }
        public DbSet<CheckNotification> CheckNotifications { get; set; }
        public DbSet<CheckNotificationRecord> CheckNotificationRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CheckResult>()
                .Property(x => x.Message)
                .IsRequired(false);

            modelBuilder
                .Entity<Check>()
                .HasIndex(x => x.Name)
                .IsUnique(true);

            modelBuilder
                .Entity<HeartGroup>()
                .HasIndex(x => x.Name)
                .IsUnique(true);

            modelBuilder
                .Entity<Heart>()
                .HasIndex(x => new { x.HeartGroupId, x.Name })
                .IsUnique(true);

            modelBuilder
                .Entity<CheckNotification>()
                .HasIndex(x => x.CheckId)
                .IsUnique(true);

            modelBuilder
                .Entity<CheckNotification>()
                .Property(x => x.Version)
                .IsConcurrencyToken();

            modelBuilder
                .Entity<CheckNotificationRecord>()
                .HasKey(x => new { x.CheckNotificationId, x.Version });

            modelBuilder
                .Entity<CheckBatch>()
                .Property(x => x.Time)
                .HasColumnName("TimeText");

            modelBuilder
                .Entity<CheckBatch>()
                .Property(x => x.Duration)
                .HasColumnName("DurationText");

            modelBuilder
                .Entity<CheckResult>()
                .Property(x => x.Time)
                .HasColumnName("TimeText");

            modelBuilder
                .Entity<CheckResult>()
                .Property(x => x.Duration)
                .HasColumnName("DurationText");

            modelBuilder
                .Entity<CheckNotification>()
                .Property(x => x.Time)
                .HasColumnName("TimeText");

            modelBuilder
                .Entity<CheckNotificationRecord>()
                .Property(x => x.Time)
                .HasColumnName("TimeText");

            modelBuilder
                .Entity<Heartbeat>()
                .Property(x => x.Time)
                .HasColumnName("TimeText");
        }
    }
}
