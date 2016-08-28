using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckRepublic.Logic.Entities.Migrations
{
    public partial class AddNameToHeartMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Hearts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hearts_HeartGroupId_Name",
                table: "Hearts",
                columns: new[] { "HeartGroupId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hearts_HeartGroupId_Name",
                table: "Hearts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Hearts");
        }
    }
}
