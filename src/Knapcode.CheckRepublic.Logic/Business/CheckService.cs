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
    public class CheckService : ICheckService
    {
        private readonly Entities.CheckContext _context;
        private readonly IEntityMapper _entityMapper;

        public CheckService(Entities.CheckContext context, IEntityMapper entityMapper)
        {
            _context = context;
            _entityMapper = entityMapper;
        }

        public async Task<IEnumerable<Check>> GetChecksAsync(int skip, int take, bool asc, CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<Entities.Check> checks = _context.Checks;
            if (asc)
            {
                checks = checks.OrderBy(x => x.CheckId);
            }
            else
            {
                checks = checks.OrderByDescending(x => x.CheckId);
            }

            var entities = await checks
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            return entities
                .Select(_entityMapper.ToBusiness)
                .ToList();
        }

        public async Task<Check> GetCheckByIdAsync(int id, CancellationToken token)
        {
            return await GetCheckAsync(x => x.CheckId == id, token);
        }

        public async Task<Check> GetCheckByNameAsync(string name, CancellationToken token)
        {
            return await GetCheckAsync(x => x.Name == name, token);
        }

        private async Task<Check> GetCheckAsync(Expression<Func<Entities.Check, bool>> predicate, CancellationToken token)
        {
            var check = await _context
                .Checks
                .FirstOrDefaultAsync(predicate, token);

            if (check == null)
            {
                return null;
            }

            return _entityMapper.ToBusiness(check);
        }
    }
}
