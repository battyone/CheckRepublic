using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckRepublic.Logic.Entities.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checks",
                columns: table => new
                {
                    CheckId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checks", x => x.CheckId);
                });

            migrationBuilder.CreateTable(
                name: "CheckBatches",
                columns: table => new
                {
                    CheckBatchId = table.Column<long>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    MachineName = table.Column<string>(nullable: true),
                    Time = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckBatches", x => x.CheckBatchId);
                });

            migrationBuilder.CreateTable(
                name: "CheckResults",
                columns: table => new
                {
                    CheckResultId = table.Column<long>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CheckBatchId = table.Column<long>(nullable: false),
                    CheckId = table.Column<int>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Time = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckResults", x => x.CheckResultId);
                    table.ForeignKey(
                        name: "FK_CheckResults_CheckBatches_CheckBatchId",
                        column: x => x.CheckBatchId,
                        principalTable: "CheckBatches",
                        principalColumn: "CheckBatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckResults_Checks_CheckId",
                        column: x => x.CheckId,
                        principalTable: "Checks",
                        principalColumn: "CheckId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_CheckBatchId",
                table: "CheckResults",
                column: "CheckBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckResults_CheckId",
                table: "CheckResults",
                column: "CheckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckResults");

            migrationBuilder.DropTable(
                name: "CheckBatches");

            migrationBuilder.DropTable(
                name: "Checks");
        }
    }
}
