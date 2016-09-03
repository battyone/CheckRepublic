using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Logic.Business.Mappers
{
    public interface IEntityMapper
    {
        CheckNotification ToBusiness(Entities.CheckNotification checkNotification);
        CheckResultType ToBusiness(Entities.CheckResultType checkResultType);
        HeartGroup ToBusiness(Entities.HeartGroup heartGroup);
        Heartbeat ToBusiness(Entities.Heartbeat heartbeat);
        CheckResult ToBusiness(Entities.CheckResult checkResult);
        CheckBatch ToBusiness(Entities.CheckBatch checkBatch);
        Check ToBusiness(Entities.Check check);
    }
}