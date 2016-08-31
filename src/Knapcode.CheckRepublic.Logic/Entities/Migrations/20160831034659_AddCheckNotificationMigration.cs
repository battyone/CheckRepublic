using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class AddCheckNotificationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckNotifications",
                columns: table => new
                {
                    CheckNotificationId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CheckId = table.Column<int>(nullable: false),
                    CheckResultId = table.Column<long>(nullable: false),
                    Time = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckNotifications", x => x.CheckNotificationId);
                    table.ForeignKey(
                        name: "FK_CheckNotifications_Checks_CheckId",
                        column: x => x.CheckId,
                        principalTable: "Checks",
                        principalColumn: "CheckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckNotifications_CheckResults_CheckResultId",
                        column: x => x.CheckResultId,
                        principalTable: "CheckResults",
                        principalColumn: "CheckResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckResultId",
                table: "CheckNotifications",
                column: "CheckResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckNotifications");
        }
    }
}
