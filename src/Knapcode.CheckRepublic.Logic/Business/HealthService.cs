using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.CheckRepublic.Logic.Utilities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class HealthService : IHealthService
    {
        private static readonly TimeSpan ErrorThreshold = TimeSpan.FromMinutes(15);

        private readonly ICheckBatchService _checkBatchService;
        private readonly ISystemClock _systemClock;

        public HealthService(ISystemClock systemClock, ICheckBatchService checkBatchService)
        {
            _systemClock = systemClock;
            _checkBatchService = checkBatchService;
        }

        public async Task<RunnerHealth> GetRunnerHealthAsync(CancellationToken token)
        {
            var checkBatch = await _checkBatchService.GetLatestCheckBatchAsync(token);

            if (checkBatch == null)
            {
                return new RunnerHealth
                {
                    IsHealthy = false,
                    Message = "No checks have been performed.",
                    LastRunTime = DateTimeOffset.MinValue
                };
            }

            var now = _systemClock.UtcNow;
            var sinceLastBatch = now - checkBatch.Time;

            if (sinceLastBatch > ErrorThreshold)
            {
                return new RunnerHealth
                {
                    IsHealthy = false,
                    Message = $"The time since the last check batch is {sinceLastBatch} ago, which is over the expected threshold of {ErrorThreshold}.",
                    LastRunTime = checkBatch.Time
                };
            }

            return new RunnerHealth
            {
                IsHealthy = true,
                Message = $"The last check batch was {sinceLastBatch} ago.",
                LastRunTime = checkBatch.Time
            };
        }
    }
}
