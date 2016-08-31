using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<CheckResult>> GetFailureCheckResultsByCheckNameAsync(string checkName, int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResultsAsync(
                x => x.Type == CheckResultType.Failure &&
                     x.Check.Name == checkName,
                skip,
                take,
                asc,
                token);
        }

        public async Task<IEnumerable<CheckResult>> GetFailureCheckResultsAsync(int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResultsAsync(
                x => x.Type != CheckResultType.Success,
                skip,
                take,
                asc,
                token);
        }

        public async Task<IEnumerable<CheckResult>> GetCheckResultsByCheckIdAsync(int checkId, int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResultsAsync(
                x => x.CheckId == checkId,
                skip,
                take,
                asc,
                token);
        }

        public async Task<IEnumerable<CheckResult>> GetCheckResultsByCheckNameAsync(string checkName, int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResultsAsync(
                x => x.Check.Name == checkName,
                skip,
                take,
                asc,
                token);
        }

        private async Task<IEnumerable<CheckResult>> GetCheckResultsAsync(
            Expression<Func<CheckResult, bool>> predicate,
            int skip,
            int take,
            bool asc,
            CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<CheckResult> checkResults = _context
                .CheckResults
                .Where(predicate);

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
