using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities.Migrations
{
    public partial class RemoveTextTimeAndDurationColumnsMigration : Migration
    {
        private const string RemoveHeartbeatColumns = @"
CREATE TABLE Heartbeats_Temporary (
    HeartbeatId INTEGER,
    HeartId INTEGER,
    Time INTEGER
);

INSERT INTO Heartbeats_Temporary
SELECT HeartbeatId, HeartId, Time
FROM Heartbeats;

DROP TABLE Heartbeats;

CREATE TABLE Heartbeats (
    HeartbeatId INTEGER NOT NULL CONSTRAINT PK_Heartbeats PRIMARY KEY AUTOINCREMENT,
    HeartId INTEGER NOT NULL,
    Time INTEGER NOT NULL,
    CONSTRAINT FK_Heartbeats_Hearts_HeartId FOREIGN KEY (HeartId) REFERENCES Hearts (HeartId) ON DELETE CASCADE
);

CREATE INDEX ""IX_Heartbeats_HeartId"" ON ""Heartbeats"" (""HeartId"");

INSERT INTO Heartbeats
SELECT HeartbeatId, HeartId, Time
FROM Heartbeats_Temporary;

DROP TABLE Heartbeats_Temporary;
";

        private const string RemoveCheckResultColumns = @"
CREATE TABLE CheckResults_Temporary (
    CheckResultId INTEGER,
    CheckBatchId INTEGER,
    CheckId INTEGER,
    Duration INTEGER,
    Message TEXT,
    Time INTEGER,
    Type INTEGER
);

INSERT INTO CheckResults_Temporary
SELECT CheckResultId, CheckBatchId, CheckId, Duration, Message, Time, Type
FROM CheckResults;

DROP TABLE CheckResults;

CREATE TABLE CheckResults (
    CheckResultId INTEGER NOT NULL CONSTRAINT PK_CheckResults PRIMARY KEY AUTOINCREMENT,
    CheckBatchId INTEGER NOT NULL,
    CheckId INTEGER NOT NULL,
    Duration INTEGER NOT NULL,
    Message TEXT,
    Time INTEGER NOT NULL,
    Type INTEGER NOT NULL,
    CONSTRAINT FK_CheckResults_CheckBatches_CheckBatchId FOREIGN KEY (CheckBatchId) REFERENCES CheckBatches (CheckBatchId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckResults_Checks_CheckId FOREIGN KEY (CheckId) REFERENCES Checks (CheckId) ON DELETE CASCADE
);

CREATE INDEX ""IX_CheckResults_CheckBatchId"" ON ""CheckResults"" (""CheckBatchId"");
CREATE INDEX ""IX_CheckResults_CheckId"" ON ""CheckResults"" (""CheckId"");

INSERT INTO CheckResults
SELECT CheckResultId, CheckBatchId, CheckId, Duration, Message, Time, Type
FROM CheckResults_Temporary;

DROP TABLE CheckResults_Temporary;
";

        private const string RemoveCheckNotificationColumns = @"
CREATE TABLE CheckNotifications_Temporary (
    CheckNotificationId INTEGER,
    CheckId INTEGER,
    CheckResultId INTEGER,
    IsHealthy INTEGER,
    Time INTEGER,
    Version INTEGER
);

INSERT INTO CheckNotifications_Temporary
SELECT CheckNotificationId, CheckId, CheckResultId, IsHealthy, Time, Version
FROM CheckNotifications;

DROP TABLE CheckNotifications;

CREATE TABLE CheckNotifications (
    CheckNotificationId INTEGER NOT NULL CONSTRAINT PK_CheckNotifications PRIMARY KEY AUTOINCREMENT,
    CheckId INTEGER NOT NULL,
    CheckResultId INTEGER NOT NULL,
    IsHealthy INTEGER NOT NULL,
    Time INTEGER NOT NULL,
    Version INTEGER NOT NULL,
    CONSTRAINT FK_CheckNotifications_Checks_CheckId FOREIGN KEY (CheckId) REFERENCES Checks (CheckId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckNotifications_CheckResults_CheckResultId FOREIGN KEY (CheckResultId) REFERENCES CheckResults (CheckResultId) ON DELETE CASCADE
);

CREATE INDEX ""IX_CheckNotifications_CheckId"" ON ""CheckNotifications"" (""CheckId"");
CREATE INDEX ""IX_CheckNotifications_CheckResultId"" ON ""CheckNotifications"" (""CheckResultId"");

INSERT INTO CheckNotifications
SELECT CheckNotificationId, CheckId, CheckResultId, IsHealthy, Time, Version
FROM CheckNotifications_Temporary;

DROP TABLE CheckNotifications_Temporary;
";

        private const string RemoveCheckNotificationRecordColumns = @"
CREATE TABLE CheckNotificationRecords_Temporary (
    CheckNotificationId INTEGER,
    Version INTEGER,    
    CheckId INTEGER,
    CheckResultId INTEGER,
    IsHealthy INTEGER,
    Time INTEGER
);

INSERT INTO CheckNotificationRecords_Temporary
SELECT CheckNotificationId, Version, CheckId, CheckResultId, IsHealthy, Time
FROM CheckNotificationRecords;

DROP TABLE CheckNotificationRecords;

CREATE TABLE CheckNotificationRecords (
    CheckNotificationId INTEGER NOT NULL,
    Version INTEGER NOT NULL,    
    CheckId INTEGER NOT NULL,
    CheckResultId INTEGER NOT NULL,
    IsHealthy INTEGER NOT NULL,    
    Time INTEGER NOT NULL,
    CONSTRAINT PK_CheckNotificationRecords PRIMARY KEY (CheckNotificationId, Version),
    CONSTRAINT FK_CheckNotificationRecords_Checks_CheckId FOREIGN KEY (CheckId) REFERENCES Checks (CheckId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckNotificationRecords_CheckNotifications_CheckNotificationId FOREIGN KEY (CheckNotificationId) REFERENCES CheckNotifications (CheckNotificationId) ON DELETE CASCADE,
    CONSTRAINT FK_CheckNotificationRecords_CheckResults_CheckResultId FOREIGN KEY (CheckResultId) REFERENCES CheckResults (CheckResultId) ON DELETE CASCADE
);

CREATE INDEX ""IX_CheckNotificationRecords_CheckId"" ON ""CheckNotificationRecords"" (""CheckId"");
CREATE INDEX ""IX_CheckNotificationRecords_CheckNotificationId"" ON ""CheckNotificationRecords"" (""CheckNotificationId"");
CREATE INDEX ""IX_CheckNotificationRecords_CheckResultId"" ON ""CheckNotificationRecords"" (""CheckResultId"");

INSERT INTO CheckNotificationRecords
SELECT CheckNotificationId, Version, CheckId, CheckResultId, IsHealthy, Time
FROM CheckNotificationRecords_Temporary;

DROP TABLE CheckNotificationRecords_Temporary;
";

        private const string RemoveCheckBatchColumns = @"
CREATE TABLE CheckBatches_Temporary (
    CheckBatchId INTEGER,
    Duration INTEGER,
    Time INTEGER
);

INSERT INTO CheckBatches_Temporary
SELECT CheckBatchId, Duration, Time
FROM CheckBatches;

DROP TABLE CheckBatches;

CREATE TABLE CheckBatches (
    CheckBatchId INTEGER NOT NULL CONSTRAINT PK_CheckBatches PRIMARY KEY AUTOINCREMENT,
    Duration INTEGER NOT NULL,
    Time INTEGER NOT NULL
);

INSERT INTO CheckBatches
SELECT CheckBatchId, Duration, Time
FROM CheckBatches_Temporary;

DROP TABLE CheckBatches_Temporary;
";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF", suppressTransaction: true);

            /*
            migrationBuilder.DropColumn(
                name: "TimeText",
                table: "Heartbeats");
            */

            migrationBuilder.Sql(RemoveHeartbeatColumns);

            /*
            migrationBuilder.DropColumn(
                name: "DurationText",
                table: "CheckResults");
            
            migrationBuilder.DropColumn(
                name: "TimeText",
                table: "CheckResults");
            */

            migrationBuilder.Sql(RemoveCheckResultColumns);

            /*
            migrationBuilder.DropColumn(
                name: "TimeText",
                table: "CheckNotificationRecords");
            */

            migrationBuilder.Sql(RemoveCheckNotificationRecordColumns);

            /*
            migrationBuilder.DropColumn(
                name: "TimeText",
                table: "CheckNotifications");  
            */

            migrationBuilder.Sql(RemoveCheckNotificationColumns);

            /*
            migrationBuilder.DropColumn(
                name: "DurationText",
                table: "CheckBatches");

            migrationBuilder.DropColumn(
                name: "TimeText",
                table: "CheckBatches");
            */

            migrationBuilder.Sql(RemoveCheckBatchColumns);

            migrationBuilder.Sql("PRAGMA foreign_keys = ON", suppressTransaction: true);

            migrationBuilder.Sql("VACUUM", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeText",
                table: "Heartbeats",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DurationText",
                table: "CheckResults",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeText",
                table: "CheckResults",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeText",
                table: "CheckNotificationRecords",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeText",
                table: "CheckNotifications",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DurationText",
                table: "CheckBatches",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeText",
                table: "CheckBatches",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
