using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckResultService : ICheckResultService
    {
        private readonly Entities.CheckContext _context;
        private readonly IEntityMapper _entityMapper;

        public CheckResultService(Entities.CheckContext context, IEntityMapper entityMapper)
        {
            _context = context;
            _entityMapper = entityMapper;
        }

        public async Task<IEnumerable<CheckResult>> GetFailureCheckResultsByCheckNameAsync(string checkName, int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResultsAsync(
                x => x.Type == Entities.CheckResultType.Failure &&
                     x.Check.Name == checkName,
                skip,
                take,
                asc,
                token);
        }

        public async Task<IEnumerable<CheckResult>> GetFailureCheckResultsByCheckIdAsync(int checkId, int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResultsAsync(
                x => x.Type == Entities.CheckResultType.Failure &&
                     x.Check.CheckId == checkId,
                skip,
                take,
                asc,
                token);
        }

        public async Task<IEnumerable<CheckResult>> GetFailureCheckResultsAsync(int skip, int take, bool asc, CancellationToken token)
        {
            return await GetCheckResultsAsync(
                x => x.Type != Entities.CheckResultType.Success,
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

        private async Task<List<CheckResult>> GetCheckResultsAsync(
            Expression<Func<Entities.CheckResult, bool>> predicate,
            int skip,
            int take,
            bool asc,
            CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<Entities.CheckResult> checkResults = _context
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

            var entities = await checkResults
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            return entities
                .Select(_entityMapper.ToBusiness)
                .ToList();
        }
    }
}
