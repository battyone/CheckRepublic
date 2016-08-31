using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class AddCheckNotificationRecordMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.CreateTable(
                name: "CheckNotificationRecords",
                columns: table => new
                {
                    CheckNotificationId = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CheckId = table.Column<int>(nullable: false),
                    CheckResultId = table.Column<long>(nullable: false),
                    Time = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckNotificationRecords", x => new { x.CheckNotificationId, x.Version });
                    table.ForeignKey(
                        name: "FK_CheckNotificationRecords_Checks_CheckId",
                        column: x => x.CheckId,
                        principalTable: "Checks",
                        principalColumn: "CheckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckNotificationRecords_CheckNotifications_CheckNotificationId",
                        column: x => x.CheckNotificationId,
                        principalTable: "CheckNotifications",
                        principalColumn: "CheckNotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckNotificationRecords_CheckResults_CheckResultId",
                        column: x => x.CheckResultId,
                        principalTable: "CheckResults",
                        principalColumn: "CheckResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "CheckNotifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotificationRecords_CheckId",
                table: "CheckNotificationRecords",
                column: "CheckId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotificationRecords_CheckNotificationId",
                table: "CheckNotificationRecords",
                column: "CheckNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotificationRecords_CheckResultId",
                table: "CheckNotificationRecords",
                column: "CheckResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CheckNotifications");

            migrationBuilder.DropTable(
                name: "CheckNotificationRecords");

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId");
        }
    }
}
