using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Knapcode.CheckRepublic.Logic.Entities;

namespace CheckRepublic.Logic.Entities.Migrations
{
    [DbContext(typeof(CheckContext))]
    [Migration("20160825141044_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.CheckEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("CheckEntities");
                });

            modelBuilder.Entity("Knapcode.CheckRepublic.Logic.Entities.CheckResultEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("CheckResultEntities");
                });
        }
    }
}
