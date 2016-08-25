using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CheckRepublic.Logic.Entities.Migrations
{
    [DbContext(typeof(CheckContext))]
    partial class CheckContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
