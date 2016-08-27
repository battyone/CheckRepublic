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
            var checkBatch = await _context
                .CheckBatches
                .Include(x => x.CheckResults)
                .ThenInclude(x => x.Check)
                .FirstOrDefaultAsync(x => x.CheckBatchId == id, token);

            if (checkBatch == null)
            {
                return null;
            }

            // These come out as empty lists, which is confusing. So clear them.
            foreach (var checkResult in checkBatch.CheckResults)
            {
                checkResult.Check.CheckResults = null;
            }

            return checkBatch;
        }
    }
}
