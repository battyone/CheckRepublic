using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckRepublic.Logic.Entities.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckResultEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Message = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckResultEntities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckEntities");

            migrationBuilder.DropTable(
                name: "CheckResultEntities");
        }
    }
}
