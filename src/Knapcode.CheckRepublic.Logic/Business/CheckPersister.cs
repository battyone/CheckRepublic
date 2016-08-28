using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckPersister : ICheckPersister
    {
        private readonly CheckContext _context;

        public CheckPersister(CheckContext context)
        {
            _context = context;
        }

        public async Task<CheckBatch> PersistBatchAsync(Runner.CheckBatch runnerBatch, CancellationToken token)
        {
            var checkNames = runnerBatch
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
            var results = runnerBatch
                .CheckResults
                .Select(x => MapToEntity(x, checkNameToId))
                .ToList();

            var batch = new CheckBatch
            {
                MachineName = Environment.MachineName,
                Time = runnerBatch.Time,
                Duration = runnerBatch.Duration,
                CheckResults = results
            };

            _context.CheckBatches.Add(batch);

            await _context.SaveChangesAsync(token);

            return batch;
        }

        private static CheckResult MapToEntity(Runner.CheckResult checkResult, Dictionary<string, Check> checkNameToId)
        {
            var check = checkNameToId[checkResult.Check.Name];
            var type = MapToEntity(checkResult.Type);

            return new CheckResult
            {
                Check = check,
                Type = type,
                Message = checkResult.Message,
                Time = checkResult.Time,
                Duration = checkResult.Duration
            };
        }

        private static CheckResultType MapToEntity(Runner.CheckResultType type)
        {
            switch (type)
            {
                case Runner.CheckResultType.Success:
                    return CheckResultType.Success;

                case Runner.CheckResultType.Failure:
                    return CheckResultType.Failure;

                default:
                    throw new NotSupportedException($"The check result type '{type}' is not yet supported by an entity.");
            }
        }
    }
}
