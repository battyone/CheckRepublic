using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class RemoveMachineNameFromCheckBatchMigration : Migration
    {
        private const string DropMachineNameColumnSql = @"
CREATE TEMPORARY TABLE CheckBatches_Temporary (CheckBatchId INTEGER, Duration TEXT, Time TEXT);

INSERT INTO CheckBatches_Temporary SELECT CheckBatchId, Duration, Time FROM CheckBatches;

DROP TABLE CheckBatches;

CREATE TABLE CheckBatches (CheckBatchId INTEGER NOT NULL CONSTRAINT PK_CheckBatches PRIMARY KEY AUTOINCREMENT, Duration TEXT NOT NULL, Time TEXT NOT NULL);

INSERT INTO CheckBatches SELECT CheckBatchId, Duration, Time FROM CheckBatches_Temporary;
            
DROP TABLE CheckBatches_Temporary;
";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF", suppressTransaction: true);

            migrationBuilder.Sql(DropMachineNameColumnSql);

            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            /*
            migrationBuilder.DropColumn(
                name: "MachineName",
                table: "CheckBatches");
            */

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId",
                unique: true);

            migrationBuilder.Sql("PRAGMA foreign_keys = ON", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications");

            migrationBuilder.AddColumn<string>(
                name: "MachineName",
                table: "CheckBatches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckNotifications_CheckId",
                table: "CheckNotifications",
                column: "CheckId");
        }
    }
}
