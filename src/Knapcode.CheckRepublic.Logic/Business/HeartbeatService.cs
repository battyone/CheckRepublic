using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class HeartbeatService : IHeartbeatService
    {
        private readonly Entities.CheckContext _context;
        private readonly IEntityMapper _entityMapper;
        private readonly ISystemClock _systemClock;

        public HeartbeatService(ISystemClock systemClock, Entities.CheckContext context, IEntityMapper entityMapper)
        {
            _systemClock = systemClock;
            _context = context;
            _entityMapper = entityMapper;
        }

        public async Task<Heartbeat> CreateHeartbeatAsync(string heartGroupName, string heartName, CancellationToken token)
        {
            var now = _systemClock.UtcNow;

            var heart = await _context
                .Hearts
                .Include(x => x.HeartGroup)
                .FirstOrDefaultAsync(x => x.HeartGroup.Name == heartGroupName && x.Name == heartName, token);

            if (heart == null)
            {
                var heartGroup = await _context
                    .HeartGroups
                    .FirstOrDefaultAsync(x => x.Name == heartGroupName, token);

                if (heartGroup == null)
                {
                    heartGroup = new Entities.HeartGroup { Name = heartGroupName };
                }

                heart = new Entities.Heart { HeartGroup = heartGroup, Name = heartName };
            }

            var heartbeat = new Entities.Heartbeat
            {
                Heart = heart,
                Time = TimeUtilities.DateTimeOffsetToLong(now)
            };

            _context.Heartbeats.Add(heartbeat);

            await _context.SaveChangesAsync(token);

            return _entityMapper.ToBusiness(heartbeat);
        }
    }
}
