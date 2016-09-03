using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckPersister : ICheckPersister
    {
        private readonly Entities.CheckContext _context;
        private readonly IEntityMapper _entityMapper;
        private readonly IRunnerMapper _runnerMapper;

        public CheckPersister(Entities.CheckContext context, IEntityMapper entityMapper, IRunnerMapper runnerMapper)
        {
            _context = context;
            _entityMapper = entityMapper;
            _runnerMapper = runnerMapper;
        }

        public async Task<CheckBatch> PersistBatchAsync(Runner.CheckBatch runnerBatch, CancellationToken token)
        {
            var checkNames = runnerBatch
                .CheckResults
                .Select(x => x.Check.Name)
                .Distinct()
                .ToList();

            // Get relevant check names
            var checkNameToCheck = await _context
                .Checks
                .Where(x => checkNames.Contains(x.Name))
                .ToDictionaryAsync(x => x.Name, x => x, token);

            // Initialize new checks
            foreach (var checkName in checkNames)
            {
                if (!checkNameToCheck.ContainsKey(checkName))
                {
                    checkNameToCheck[checkName] = new Entities.Check { Name = checkName };
                }
            }

            // Initialize the batch entity
            var batch = _runnerMapper.ToEntity(runnerBatch);

            batch.CheckResults = runnerBatch
                .CheckResults
                .Select(x => ToEntity(x, checkNameToCheck))
                .ToList();
            
            // Persist the batch
            _context.CheckBatches.Add(batch);

            await _context.SaveChangesAsync(token);

            return _entityMapper.ToBusiness(batch);
        }

        private Entities.CheckResult ToEntity(Runner.CheckResult checkResult, Dictionary<string, Entities.Check> checkNameToCheck)
        {
            var entity = _runnerMapper.ToEntity(checkResult);

            entity.Check = checkNameToCheck[checkResult.Check.Name];

            return entity;
        }
    }
}
