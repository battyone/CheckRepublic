using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class RemoveDuplicateConcurrencyTokenMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId");
        }
    }
}
