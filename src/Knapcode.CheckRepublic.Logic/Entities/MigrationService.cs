using System.Collections.Generic;
using System.Linq;
using Knapcode.CheckRepublic.Logic.Entities.DataMigrations;
using Knapcode.CheckRepublic.Logic.Entities.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class MigrationService : IMigrationService
    {
        private readonly CheckContext _context;

        public MigrationService(CheckContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            var historyRepository = _context.Database.GetService<IHistoryRepository>();
            var migrationAssembly = _context.Database.GetService<IMigrationsAssembly>();

            var appliedMigrations = historyRepository.GetAppliedMigrations().Select(x => x.MigrationId);
            var allMigrations = migrationAssembly.Migrations.Select(x => x.Key);
            var migrationsToApply = allMigrations.Except(appliedMigrations).OrderBy(x => x).ToList();

            var migrator = _context.Database.GetService<IMigrator>();
            migrator.Migrate();

            PopulateIntegerTimeAndDurationColumns(migrationsToApply);
        }

        private void PopulateIntegerTimeAndDurationColumns(List<string> migrationsToApply)
        {
            var wasApplied = migrationsToApply.Any(x => x.EndsWith(nameof(AddIntegerTimeAndDurationColumnsMigration)));

            if (!wasApplied)
            {
                return;
            }

            var migration = new AddIntegerTimeAndDurationColumnsDataMigration();
            migration.Up(_context);
        }
    }
}
