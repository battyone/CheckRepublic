using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class AddIntegerTimeAndDurationColumnsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.AddColumn<long>(
                name: "Time",
                table: "Heartbeats",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "CheckResults",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Time",
                table: "CheckResults",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Time",
                table: "CheckNotificationRecords",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Time",
                table: "CheckNotifications",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "CheckBatches",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Time",
                table: "CheckBatches",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Heartbeats");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "CheckResults");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "CheckResults");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "CheckNotificationRecords");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "CheckNotifications");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "CheckBatches");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "CheckBatches");

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId");
        }
    }
}
