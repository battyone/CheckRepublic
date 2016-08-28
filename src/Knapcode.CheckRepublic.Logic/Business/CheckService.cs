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
    public class CheckService : ICheckService
    {
        private readonly CheckContext _context;

        public CheckService(CheckContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Check>> GetChecksAsync(int skip, int take, bool asc, CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<Check> checks = _context.Checks;
            if (asc)
            {
                checks = checks.OrderBy(x => x.CheckId);
            }
            else
            {
                checks = checks.OrderByDescending(x => x.CheckId);
            }

            return await checks
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }

        public async Task<Check> GetCheckByIdAsync(int id, CancellationToken token)
        {
            return await GetCheckAsync(x => x.CheckId == id, token);
        }

        public async Task<Check> GetCheckByNameAsync(string name, CancellationToken token)
        {
            return await GetCheckAsync(x => x.Name == name, token);
        }

        public async Task<IEnumerable<CheckResult>> GetCheckResultsByIdAsync(int checkId, int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResults(x => x.CheckId == checkId, skip, take, asc, token);
        }

        public async Task<IEnumerable<CheckResult>> GetCheckResultsByNameAsync(string checkName, int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResults(x => x.Check.Name == checkName, skip, take, asc, token);
        }

        private async Task<Check> GetCheckAsync(Expression<Func<Check, bool>> predicate, CancellationToken token)
        {
            return await _context
                .Checks
                .FirstOrDefaultAsync(predicate, token);
        }

        private async Task<IEnumerable<CheckResult>> GetCheckResults(
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
                checkResults = checkResults.OrderBy(x => x.CheckResultId);
            }
            else
            {
                checkResults = checkResults.OrderByDescending(x => x.CheckResultId);
            }

            return await checkResults
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
