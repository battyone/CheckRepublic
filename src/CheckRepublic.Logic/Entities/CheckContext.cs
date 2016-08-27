﻿using Microsoft.EntityFrameworkCore;

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
        }
    }
}