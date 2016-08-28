using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class PoGoNotificationsHeartbeatCheck : ICheck
    {
        private const string HeartGroupName = "PoGoNotifications.PokemonEncounter";
        private static readonly TimeSpan ErrorThreshold = TimeSpan.FromMinutes(30);

        private readonly IHeartbeatCheck _heartbeatCheck;

        public PoGoNotificationsHeartbeatCheck(IHeartbeatCheck heartbeatCheck)
        {
            _heartbeatCheck = heartbeatCheck;
        }

        public string Name => "PoGoNotifications Heartbeat";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _heartbeatCheck.ExecuteAsync(
                HeartGroupName,
                ErrorThreshold,
                token);
        }
    }
}
