using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class HeartbeatService : IHeartbeatService
    {
        private readonly CheckContext _context;
        private readonly ISystemClock _systemClock;

        public HeartbeatService(ISystemClock systemClock, CheckContext context)
        {
            _systemClock = systemClock;
            _context = context;
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
                    heartGroup = new HeartGroup { Name = heartGroupName };
                }

                heart = new Heart { HeartGroup = heartGroup, Name = heartName };
            }

            var heartbeat = new Heartbeat { Heart = heart, Time = now };

            _context.Heartbeats.Add(heartbeat);

            await _context.SaveChangesAsync(token);

            return heartbeat;
        }
    }
}
