using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public class CheckService
    {
        private readonly ICheckBatchRunner _batchRunner;
        private readonly CheckContext _context;

        public CheckService(CheckContext context, ICheckBatchRunner batchRunner)
        {
            _context = context;
            _batchRunner = batchRunner;
        }

        public async Task CheckAsync(IEnumerable<ICheck> checks, CancellationToken token)
        {
            var batch = await _batchRunner.ExecuteAsync(checks, token);

            await PersistBatchAsync(batch, token);
        }

        private async Task PersistBatchAsync(CheckBatch batch, CancellationToken token)
        {
            var checkNames = batch
                .CheckResults
                .Select(x => x.Check.Name)
                .Distinct()
                .ToList();

            // Get relevant check names
            var checkNameToId = await _context
                .Checks
                .Where(x => checkNames.Contains(x.Name))
                .ToDictionaryAsync(x => x.Name, x => x, token);

            // Initialize new checks
            foreach (var checkName in checkNames)
            {
                if (!checkNameToId.ContainsKey(checkName))
                {
                    checkNameToId[checkName] = new Check { Name = checkName };
                }
            }

            // Initialize the entities
            var resultEntities = batch
                .CheckResults
                .Select(x => MapToEntity(x, checkNameToId))
                .ToList();

            var batchEntity = new Entities.CheckBatch
            {
                MachineName = Environment.MachineName,
                Time = batch.Time,
                Duration = batch.Duration,
                CheckResults = resultEntities
            };

            _context.CheckBatches.Add(batchEntity);
            await _context.SaveChangesAsync(token);
        }

        private static Entities.CheckResult MapToEntity(CheckResult checkResult, Dictionary<string, Check> checkNameToId)
        {
            var check = checkNameToId[checkResult.Check.Name];
            var type = MapToEntity(checkResult.Type);

            return new Entities.CheckResult
            {
                Check = check,
                Type = type,
                Message = checkResult.Message,
                Time = checkResult.Time,
                Duration = checkResult.Duration
            };
        }

        private static Entities.CheckResultType MapToEntity(CheckResultType type)
        {
            switch (type)
            {
                case CheckResultType.Success:
                    return Entities.CheckResultType.Success;

                case CheckResultType.Failure:
                    return Entities.CheckResultType.Failure;

                default:
                    throw new NotSupportedException($"The check result type '{type}' is not yet supported by an entity.");
            }
        }
    }
}
