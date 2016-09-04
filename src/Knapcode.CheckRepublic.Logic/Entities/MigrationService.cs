using System.Diagnostics;
using System.Linq;
using Knapcode.CheckRepublic.Logic.Entities.DataMigrations;
using Knapcode.CheckRepublic.Logic.Entities.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class MigrationService : IMigrationService
    {
        private readonly CheckContext _context;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(CheckContext context, ILogger<MigrationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Migrate()
        {
            var historyRepository = _context.Database.GetService<IHistoryRepository>();
            var migrationAssembly = _context.Database.GetService<IMigrationsAssembly>();

            var appliedMigrations = historyRepository.GetAppliedMigrations().Select(x => x.MigrationId);
            var allMigrations = migrationAssembly.Migrations.Select(x => x.Key);
            var migrationsToApply = allMigrations.Except(appliedMigrations).OrderBy(x => x).ToList();

            var migrator = _context.Database.GetService<IMigrator>();

            foreach (var migrationId in migrationsToApply)
            {
                _logger.LogInformation("Executing migration '{migrationId}'.", migrationId);
                var migrationStopwatch = Stopwatch.StartNew();
                migrator.Migrate(migrationId);
                _logger.LogInformation("Migration '{migrationId}' completed in {duration}.", migrationId, migrationStopwatch.Elapsed);

                var dataMigration = GetDataMigration(migrationId);
                if (dataMigration != null)
                {
                    _logger.LogInformation("Executing data migration '{migrationId}'.", migrationId);
                    var dataMigrationStopwatch = Stopwatch.StartNew();
                    dataMigration.Up(_context);
                    _logger.LogInformation("Data migration '{migrationId}' completed in {duration}.", migrationId, dataMigrationStopwatch.Elapsed);
                }
            }
        }
        
        private IDataMigration GetDataMigration(string migrationId)
        {
            if (migrationId.EndsWith("_" + nameof(AddIntegerTimeAndDurationColumnsMigration)))
            {
                return new AddIntegerTimeAndDurationColumnsDataMigration();
            }

            return null;
        }
    }
}
