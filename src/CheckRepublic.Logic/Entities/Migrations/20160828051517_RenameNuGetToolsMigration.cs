using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckRepublic.Logic.Entities.Migrations
{
    public partial class RenameNuGetToolsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Checks SET Name = 'NuGet Tools Up' WHERE Name = 'NuGet Tools'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Checks SET Name = 'NuGet Tools' WHERE Name = 'NuGet Tools Up'");
        }
    }
}
