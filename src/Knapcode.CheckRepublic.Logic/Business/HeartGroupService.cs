using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class HeartGroupService : IHeartGroupService
    {
        private readonly CheckContext _context;

        public HeartGroupService(CheckContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HeartGroup>> GetHeartGroupsAsync(int skip, int take, bool asc, CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<HeartGroup> heartGroups = _context.HeartGroups;
            if (asc)
            {
                heartGroups = heartGroups.OrderBy(x => x.HeartGroupId);
            }
            else
            {
                heartGroups = heartGroups.OrderByDescending(x => x.HeartGroupId);
            }

            return await heartGroups
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }

        public async Task<IEnumerable<Heartbeat>> GetHeartbeatsByHeartGroupName(
            string heartGroupName,
            int skip,
            int take,
            bool asc,
            CancellationToken token)
        {
            ValidationUtility.ValidatePagingParameters(skip, take);

            IQueryable<Heartbeat> heartbeats = _context
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

            return await heartbeats
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
