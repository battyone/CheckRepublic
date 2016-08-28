using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckBatchService : ICheckBatchService
    {
        private readonly CheckContext _context;

        public CheckBatchService(CheckContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CheckBatch>> GetCheckBatchesAsync(int skip, int take, bool asc, CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<CheckBatch> checkBatches = _context.CheckBatches;
            if (asc)
            {
                checkBatches = checkBatches.OrderBy(x => x.CheckBatchId);
            }
            else
            {
                checkBatches = checkBatches.OrderByDescending(x => x.CheckBatchId);
            }

            return await checkBatches
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
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

        private async Task<CheckBatch> GetCheckBatchAsync(Func<IQueryable<CheckBatch>, CancellationToken, Task<CheckBatch>> selectAsync, CancellationToken token)
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

            return checkBatch;
        }
    }
}
