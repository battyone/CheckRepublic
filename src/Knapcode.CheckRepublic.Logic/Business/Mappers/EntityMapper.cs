using System;
using System.Collections.Generic;
using System.Linq;
using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Logic.Business.Mappers
{
    public class EntityMapper : IEntityMapper
    {
        public Check ToBusiness(Entities.Check check)
        {
            return new Check
            {
                CheckId = check.CheckId,
                Name = check.Name
            };
        }

        public CheckBatch ToBusiness(Entities.CheckBatch checkBatch)
        {
            return new CheckBatch
            {
                CheckBatchId = checkBatch.CheckBatchId,
                Time = checkBatch.TimeText,
                Duration = checkBatch.DurationText,
                CheckResults = checkBatch.CheckResults != null ? ToBusiness(checkBatch.CheckResults) : null
            };
        }

        public CheckResult ToBusiness(Entities.CheckResult checkResult)
        {
            return new CheckResult
            {
                CheckResultId = checkResult.CheckResultId,
                CheckBatchId = checkResult.CheckBatchId,
                CheckId = checkResult.CheckId,
                Type = ToBusiness(checkResult.Type),
                Message = checkResult.Message,
                Time = checkResult.TimeText,
                Duration = checkResult.DurationText,
                Check = checkResult.Check != null ? ToBusiness(checkResult.Check) : null
            };
        }

        public CheckNotification ToBusiness(Entities.CheckNotification checkNotification)
        {
            return new CheckNotification
            {
                CheckNotificationId = checkNotification.CheckNotificationId,
                CheckId = checkNotification.CheckId,
                CheckResultId = checkNotification.CheckResultId,
                Version = checkNotification.Version,
                IsHealthy = checkNotification.IsHealthy,
                Time = checkNotification.TimeText,
                CheckResult = ToBusiness(checkNotification.CheckResult)
            };
        }

        public Heartbeat ToBusiness(Entities.Heartbeat heartbeat)
        {
            return new Heartbeat
            {
                HeartbeatId = heartbeat.HeartbeatId,
                HeartId = heartbeat.HeartId,
                Time = heartbeat.TimeText
            };
        }

        public HeartGroup ToBusiness(Entities.HeartGroup heartGroup)
        {
            return new HeartGroup
            {
                HeartGroupId = heartGroup.HeartGroupId,
                Name = heartGroup.Name
            };
        }

        public CheckResultType ToBusiness(Entities.CheckResultType checkResultType)
        {
            switch (checkResultType)
            {
                case Entities.CheckResultType.Success:
                    return CheckResultType.Success;

                case Entities.CheckResultType.Failure:
                    return CheckResultType.Failure;

                default:
                    throw new ArgumentOutOfRangeException(nameof(checkResultType));
            }
        }

        private List<CheckResult> ToBusiness(List<Entities.CheckResult> checkResults)
        {
            return checkResults.Select(ToBusiness).ToList();
        }
    }
}
