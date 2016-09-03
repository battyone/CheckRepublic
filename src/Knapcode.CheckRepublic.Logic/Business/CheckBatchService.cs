using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckBatchService : ICheckBatchService
    {
        private readonly Entities.CheckContext _context;
        private readonly IEntityMapper _entityMapper;

        public CheckBatchService(Entities.CheckContext context, IEntityMapper entityMapper)
        {
            _context = context;
            _entityMapper = entityMapper;
        }

        public async Task<IEnumerable<CheckBatch>> GetCheckBatchesAsync(int skip, int take, bool asc, CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<Entities.CheckBatch> checkBatches = _context.CheckBatches;

            if (asc)
            {
                checkBatches = checkBatches.OrderBy(x => x.CheckBatchId);
            }
            else
            {
                checkBatches = checkBatches.OrderByDescending(x => x.CheckBatchId);
            }

            var entities = await checkBatches
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            return entities
                .Select(_entityMapper.ToBusiness)
                .ToList();
        }

        public async Task<CheckBatch> GetCheckBatchByIdAsync(int id, CancellationToken token)
        {
            return await GetCheckBatchAsync(
                (x, t) => x.FirstOrDefaultAsync(b => b.CheckBatchId == id, t),
                token);
        }

        public async Task<CheckBatch> GetLatestCheckBatchAsync(CancellationToken token)
        {
            return await GetCheckBatchAsync(
                (x, t) => x.OrderByDescending(b => b.CheckBatchId).FirstOrDefaultAsync(t),
                token);
        }

        private async Task<CheckBatch> GetCheckBatchAsync(
            Func<IQueryable<Entities.CheckBatch>, CancellationToken, Task<Entities.CheckBatch>> selectAsync,
            CancellationToken token)
        {
            var checkBatches = _context
                .CheckBatches
                .Include(x => x.CheckResults)
                .ThenInclude(x => x.Check);

            var checkBatch = await selectAsync(checkBatches, token);

            if (checkBatch == null)
            {
                return null;
            }

            return _entityMapper.ToBusiness(checkBatch);
        }
    }
}
