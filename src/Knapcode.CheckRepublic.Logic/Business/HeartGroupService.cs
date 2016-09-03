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
    public class HeartGroupService : IHeartGroupService
    {
        private readonly Entities.CheckContext _context;
        private readonly IEntityMapper _entityMapper;

        public HeartGroupService(Entities.CheckContext context, IEntityMapper entityMapper)
        {
            _context = context;
            _entityMapper = entityMapper;
        }

        public async Task<IEnumerable<HeartGroup>> GetHeartGroupsAsync(int skip, int take, bool asc, CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<Entities.HeartGroup> heartGroups = _context.HeartGroups;
            if (asc)
            {
                heartGroups = heartGroups.OrderBy(x => x.HeartGroupId);
            }
            else
            {
                heartGroups = heartGroups.OrderByDescending(x => x.HeartGroupId);
            }

            var entities = await heartGroups
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            return entities
                .Select(_entityMapper.ToBusiness)
                .ToList();
        }

        public async Task<IEnumerable<Heartbeat>> GetHeartbeatsByHeartGroupName(
            string heartGroupName,
            int skip,
            int take,
            bool asc,
            CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<Entities.Heartbeat> heartbeats = _context
                .Heartbeats
                .Where(x => x.Heart.HeartGroup.Name == heartGroupName);

            if (asc)
            {
                heartbeats = heartbeats.OrderBy(x => x.HeartbeatId);
            }
            else
            {
                heartbeats = heartbeats.OrderByDescending(x => x.HeartbeatId);
            }

            var entities = await heartbeats
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            return entities
                .Select(_entityMapper.ToBusiness)
                .ToList();
        }
    }
}
