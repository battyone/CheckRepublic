using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckResultService : ICheckResultService
    {
        private readonly CheckContext _context;

        public CheckResultService(CheckContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CheckResult>> GetFailedCheckResultsAsync(int skip, int take, bool asc, CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<CheckResult> checkResults = _context
                .CheckResults
                .Where(x => x.Type == CheckResultType.Failure);

            if (asc)
            {
                checkResults = checkResults.OrderBy(x => x.CheckBatchId);
            }
            else
            {
                checkResults = checkResults.OrderByDescending(x => x.CheckBatchId);
            }

            return await checkResults
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
