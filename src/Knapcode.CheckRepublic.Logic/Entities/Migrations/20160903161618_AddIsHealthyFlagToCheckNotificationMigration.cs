using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class AddIsHealthyFlagToCheckNotificationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsHealthy",
                table: "CheckNotificationRecords",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHealthy",
                table: "CheckNotifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId",
                unique: true);

            migrationBuilder.Sql("UPDATE CheckNotifications SET IsHealthy = 0");

            migrationBuilder.Sql("UPDATE CheckNotificationRecords SET IsHealthy = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.DropColumn(
                name: "IsHealthy",
                table: "CheckNotificationRecords");

            migrationBuilder.DropColumn(
                name: "IsHealthy",
                table: "CheckNotifications");

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId");
        }
    }
}
