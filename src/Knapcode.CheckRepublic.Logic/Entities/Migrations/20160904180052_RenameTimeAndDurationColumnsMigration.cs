using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class RenameTimeAndDurationColumnsMigration : Migration
    {
        private const string RenameHeartbeatColumns = @"
CREATE TABLE Heartbeats_Temporary (
    HeartbeatId INTEGER,
    HeartId INTEGER,
    {0} TEXT
);

INSERT INTO Heartbeats_Temporary
SELECT HeartbeatId, HeartId, {0}
FROM Heartbeats;

DROP TABLE Heartbeats;

CREATE TABLE Heartbeats (
    HeartbeatId INTEGER NOT NULL CONSTRAINT PK_Heartbeats PRIMARY KEY AUTOINCREMENT,
    HeartId INTEGER NOT NULL,
    {1} TEXT NOT NULL,
    CONSTRAINT FK_Heartbeats_Hearts_HeartId FOREIGN KEY (HeartId) REFERENCES Hearts (HeartId) ON DELETE CASCADE
);

CREATE INDEX ""IX_Heartbeats_HeartId"" ON ""Heartbeats"" (""HeartId"");

INSERT INTO Heartbeats
SELECT HeartbeatId, HeartId, {0}
FROM Heartbeats_Temporary;

DROP TABLE Heartbeats_Temporary;
";

        private const string RenameCheckResultColumns = @"
CREATE TABLE CheckResults_Temporary (
    CheckResultId INTEGER,
    CheckBatchId INTEGER,
    CheckId INTEGER,
    {1} TEXT,
    Message TEXT,
    {0} TEXT,
    Type INTEGER
);

INSERT INTO CheckResults_Temporary
SELECT CheckResultId, CheckBatchId, CheckId, {1}, Message, {0}, Type
FROM CheckResults;

DROP TABLE CheckResults;

CREATE TABLE CheckResults (
    CheckResultId INTEGER NOT NULL CONSTRAINT PK_CheckResults PRIMARY KEY AUTOINCREMENT,
    CheckBatchId INTEGER NOT NULL,
    CheckId INTEGER NOT NULL,
    {3} TEXT NOT NULL,
    Message TEXT,
    {2} TEXT NOT NULL,
    Type INTEGER NOT NULL,
    CONSTRAINT FK_CheckResults_CheckBatches_CheckBatchId FOREIGN KEY (CheckBatchId) REFERENCES CheckBatches (CheckBatchId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckResults_Checks_CheckId FOREIGN KEY (CheckId) REFERENCES Checks (CheckId) ON DELETE CASCADE
);

CREATE INDEX ""IX_CheckResults_CheckBatchId"" ON ""CheckResults"" (""CheckBatchId"");
CREATE INDEX ""IX_CheckResults_CheckId"" ON ""CheckResults"" (""CheckId"");

INSERT INTO CheckResults
SELECT CheckResultId, CheckBatchId, CheckId, {1}, Message, {0}, Type
FROM CheckResults_Temporary;

DROP TABLE CheckResults_Temporary;
";

        private const string RenameCheckNotificationColumns = @"
CREATE TABLE CheckNotifications_Temporary (
    CheckNotificationId INTEGER,
    CheckId INTEGER,
    CheckResultId INTEGER,
    {0} TEXT,
    Version INTEGER,
    IsHealthy INTEGER
);

INSERT INTO CheckNotifications_Temporary
SELECT CheckNotificationId, CheckId, CheckResultId, {0}, Version, IsHealthy
FROM CheckNotifications;

DROP TABLE CheckNotifications;

CREATE TABLE CheckNotifications (
    CheckNotificationId INTEGER NOT NULL CONSTRAINT PK_CheckNotifications PRIMARY KEY AUTOINCREMENT,
    CheckId INTEGER NOT NULL,
    CheckResultId INTEGER NOT NULL,
    {1} TEXT NOT NULL,
    Version INTEGER NOT NULL DEFAULT 0,
    IsHealthy INTEGER NOT NULL DEFAULT 0,
    CONSTRAINT FK_CheckNotifications_Checks_CheckId FOREIGN KEY (CheckId) REFERENCES Checks (CheckId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckNotifications_CheckResults_CheckResultId FOREIGN KEY (CheckResultId) REFERENCES CheckResults (CheckResultId) ON DELETE CASCADE
);

CREATE UNIQUE INDEX ""IX_CheckNotifications_CheckId"" ON ""CheckNotifications"" (""CheckId"");
CREATE INDEX ""IX_CheckNotifications_CheckResultId"" ON ""CheckNotifications"" (""CheckResultId"");

INSERT INTO CheckNotifications
SELECT CheckNotificationId, CheckId, CheckResultId, {0}, Version, IsHealthy
FROM CheckNotifications_Temporary;

DROP TABLE CheckNotifications_Temporary;
";

        private const string RenameCheckNotificationRecordColumns = @"
CREATE TABLE CheckNotificationRecords_Temporary (
    CheckNotificationId INTEGER,
    Version INTEGER,    
    CheckId INTEGER,
    CheckResultId INTEGER,
    {0} TEXT,
    IsHealthy INTEGER
);

INSERT INTO CheckNotificationRecords_Temporary
SELECT CheckNotificationId, Version, CheckId, CheckResultId, {0}, IsHealthy
FROM CheckNotificationRecords;

DROP TABLE CheckNotificationRecords;

CREATE TABLE CheckNotificationRecords (
    CheckNotificationId INTEGER NOT NULL,
    Version INTEGER NOT NULL,    
    CheckId INTEGER NOT NULL,
    CheckResultId INTEGER NOT NULL,
    {1} TEXT NOT NULL,
    IsHealthy INTEGER NOT NULL DEFAULT 0,
    CONSTRAINT PK_CheckNotificationRecords PRIMARY KEY (CheckNotificationId, Version),
    CONSTRAINT FK_CheckNotificationRecords_Checks_CheckId FOREIGN KEY (CheckId) REFERENCES Checks (CheckId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckNotificationRecords_CheckNotifications_CheckNotificationId FOREIGN KEY (CheckNotificationId) REFERENCES CheckNotifications (CheckNotificationId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckNotificationRecords_CheckResults_CheckResultId FOREIGN KEY (CheckResultId) REFERENCES CheckResults (CheckResultId) ON DELETE CASCADE
);

CREATE INDEX ""IX_CheckNotificationRecords_CheckId"" ON ""CheckNotificationRecords"" (""CheckId"");
CREATE INDEX ""IX_CheckNotificationRecords_CheckNotificationId"" ON ""CheckNotificationRecords"" (""CheckNotificationId"");
CREATE INDEX ""IX_CheckNotificationRecords_CheckResultId"" ON ""CheckNotificationRecords"" (""CheckResultId"");

INSERT INTO CheckNotificationRecords
SELECT CheckNotificationId, Version, CheckId, CheckResultId, {0}, IsHealthy
FROM CheckNotificationRecords_Temporary;

DROP TABLE CheckNotificationRecords_Temporary;
";

        private const string RenameCheckBatchColumns = @"
CREATE TABLE CheckBatches_Temporary (
    CheckBatchId INTEGER,
    {1} TEXT,
    {0} TEXT
);

INSERT INTO CheckBatches_Temporary
SELECT CheckBatchId, {1}, {0}
FROM CheckBatches;

DROP TABLE CheckBatches;

CREATE TABLE CheckBatches (
    CheckBatchId INTEGER NOT NULL CONSTRAINT PK_CheckBatches PRIMARY KEY AUTOINCREMENT,
    {3} TEXT NOT NULL,
    {2} TEXT NOT NULL
);

INSERT INTO CheckBatches
SELECT CheckBatchId, {1}, {0}
FROM CheckBatches_Temporary;

DROP TABLE CheckBatches_Temporary;
";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF", suppressTransaction: true);

            /*
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Heartbeats",
                newName: "TimeText");
            */

            migrationBuilder.Sql(string.Format(RenameHeartbeatColumns, "Time", "TimeText"));

            /*
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "CheckResults",
                newName: "TimeText");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "CheckResults",
                newName: "DurationText");
            */

            migrationBuilder.Sql(string.Format(RenameCheckResultColumns, "Time", "Duration", "TimeText", "DurationText"));

            /*
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "CheckNotificationRecords",
                newName: "TimeText");
            */

            migrationBuilder.Sql(string.Format(RenameCheckNotificationRecordColumns, "Time", "TimeText"));

            /*
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "CheckNotifications",
                newName: "TimeText");
            */

            migrationBuilder.Sql(string.Format(RenameCheckNotificationColumns, "Time", "TimeText"));

            /*
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "CheckBatches",
                newName: "TimeText");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "CheckBatches",
                newName: "DurationText");
            */

            migrationBuilder.Sql(string.Format(RenameCheckBatchColumns, "Time", "Duration", "TimeText", "DurationText"));

            migrationBuilder.Sql("PRAGMA foreign_keys = ON", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF", suppressTransaction: true);

            /*
            migrationBuilder.RenameColumn(
                name: "TimeText",
                table: "Heartbeats",
                newName: "Time");
            */

            migrationBuilder.Sql(string.Format(RenameHeartbeatColumns, "TimeText", "Time"));

            /*
            migrationBuilder.RenameColumn(
                name: "TimeText",
                table: "CheckResults",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "DurationText",
                table: "CheckResults",
                newName: "Duration");
            */

            migrationBuilder.Sql(string.Format(RenameCheckResultColumns, "TimeText", "DurationText", "Time", "Duration"));

            /*
            migrationBuilder.RenameColumn(
                name: "TimeText",
                table: "CheckNotificationRecords",
                newName: "Time");
            */

            migrationBuilder.Sql(string.Format(RenameCheckNotificationRecordColumns, "TimeText", "Time"));

            /*
            migrationBuilder.RenameColumn(
                name: "TimeText",
                table: "CheckNotifications",
                newName: "Time");
            */

            migrationBuilder.Sql(string.Format(RenameCheckNotificationColumns, "TimeText", "Time"));

            /*
            migrationBuilder.RenameColumn(
                name: "TimeText",
                table: "CheckBatches",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "DurationText",
                table: "CheckBatches",
                newName: "Duration");
            */

            migrationBuilder.Sql(string.Format(RenameCheckBatchColumns, "TimeText", "DurationText", "Time", "Duration"));

            migrationBuilder.Sql("PRAGMA foreign_keys = ON", suppressTransaction: true);
        }
    }
}
