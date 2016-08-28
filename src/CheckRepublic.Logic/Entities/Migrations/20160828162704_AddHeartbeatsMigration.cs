using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckRepublic.Logic.Entities.Migrations
{
    public partial class AddHeartbeatsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeartGroups",
                columns: table => new
                {
                    HeartGroupId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeartGroups", x => x.HeartGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Hearts",
                columns: table => new
                {
                    HeartId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    HeartGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hearts", x => x.HeartId);
                    table.ForeignKey(
                        name: "FK_Hearts_HeartGroups_HeartGroupId",
                        column: x => x.HeartGroupId,
                        principalTable: "HeartGroups",
                        principalColumn: "HeartGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Heartbeats",
                columns: table => new
                {
                    HeartbeatId = table.Column<long>(nullable: false)
                        .Annotation("Autoincrement", true),
                    HeartId = table.Column<int>(nullable: false),
                    Time = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heartbeats", x => x.HeartbeatId);
                    table.ForeignKey(
                        name: "FK_Heartbeats_Hearts_HeartId",
                        column: x => x.HeartId,
                        principalTable: "Hearts",
                        principalColumn: "HeartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hearts_HeartGroupId",
                table: "Hearts",
                column: "HeartGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Heartbeats_HeartId",
                table: "Heartbeats",
                column: "HeartId");

            migrationBuilder.CreateIndex(
                name: "IX_HeartGroups_Name",
                table: "HeartGroups",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Heartbeats");

            migrationBuilder.DropTable(
                name: "Hearts");

            migrationBuilder.DropTable(
                name: "HeartGroups");
        }
    }
}
