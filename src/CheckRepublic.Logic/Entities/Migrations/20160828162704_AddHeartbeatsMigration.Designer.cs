using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Knapcode.CheckRepublic.Logic.Entities;

namespace CheckRepublic.Logic.Entities.Migrations
{
    [DbContext(typeof(CheckContext))]
    [Migration("20160828162704_AddHeartbeatsMigration")]
    partial class AddHeartbeatsMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.Check", b =>
                {
                    b.Property<int>("CheckId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CheckId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Checks");
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.CheckBatch", b =>
                {
                    b.Property<long>("CheckBatchId")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("Duration");

                    b.Property<string>("MachineName");

                    b.Property<DateTimeOffset>("Time");

                    b.HasKey("CheckBatchId");

                    b.ToTable("CheckBatches");
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.CheckResult", b =>
                {
                    b.Property<long>("CheckResultId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CheckBatchId");

                    b.Property<int>("CheckId");

                    b.Property<TimeSpan>("Duration");

                    b.Property<string>("Message");

                    b.Property<DateTimeOffset>("Time");

                    b.Property<int>("Type");

                    b.HasKey("CheckResultId");

                    b.HasIndex("CheckBatchId");

                    b.HasIndex("CheckId");

                    b.ToTable("CheckResults");
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.Heart", b =>
                {
                    b.Property<int>("HeartId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HeartGroupId");

                    b.HasKey("HeartId");

                    b.HasIndex("HeartGroupId");

                    b.ToTable("Hearts");
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.Heartbeat", b =>
                {
                    b.Property<long>("HeartbeatId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HeartId");

                    b.Property<DateTimeOffset>("Time");

                    b.HasKey("HeartbeatId");

                    b.HasIndex("HeartId");

                    b.ToTable("Heartbeats");
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.HeartGroup", b =>
                {
                    b.Property<int>("HeartGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("HeartGroupId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("HeartGroups");
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.CheckResult", b =>
                {
                    b.HasOne("Knapcode.CheckRepublic.Logic.Entities.CheckBatch", "CheckBatch")
                        .WithMany("CheckResults")
                        .HasForeignKey("CheckBatchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Knapcode.CheckRepublic.Logic.Entities.Check", "Check")
                        .WithMany("CheckResults")
                        .HasForeignKey("CheckId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.Heart", b =>
                {
                    b.HasOne("Knapcode.CheckRepublic.Logic.Entities.HeartGroup", "HeartGroup")
                        .WithMany("Hearts")
                        .HasForeignKey("HeartGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.Heartbeat", b =>
                {
                    b.HasOne("Knapcode.CheckRepublic.Logic.Entities.Heart", "Heart")
                        .WithMany("Heartbeats")
                        .HasForeignKey("HeartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
