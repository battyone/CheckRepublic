using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public class HeartbeatCheck : IHeartbeatCheck
    {
        private readonly IHeartGroupService _service;
        private readonly ISystemClock _systemClock;

        public HeartbeatCheck(ISystemClock systemClock, IHeartGroupService service)
        {
            _systemClock = systemClock;
            _service = service;
        }

        public async Task<CheckResultData> ExecuteAsync(string heartGroupName, TimeSpan errorThreshold, CancellationToken token)
        {
            var now = _systemClock.UtcNow;

            var heartbeats = await _service.GetHeartbeatsByHeartGroupName(
                heartGroupName,
                skip: 0,
                take: 1,
                asc: false,
                token: token);

            var heartbeat = heartbeats.FirstOrDefault();

            if (heartbeat == null)
            {
                return new CheckResultData
                {
                    Type = CheckResultType.Failure,
                    Message = $"There is no heartbeats for heart group '{heartGroupName}'."
                };
            }

            var sinceLastHeartbeat = now - heartbeat.Time;

            if (sinceLastHeartbeat > errorThreshold)
            {
                return new CheckResultData
                {
                    Type = CheckResultType.Failure,
                    Message = $"The last heartbeat was at {heartbeat.Time} ({sinceLastHeartbeat} ago) which is greater than the error threshold, {errorThreshold}."
                };
            }

            return new CheckResultData
            {
                Type = CheckResultType.Success
            };
        }
    }
}
