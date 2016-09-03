using System;

namespace Knapcode.CheckRepublic.Logic.Business.Mappers
{
    public class RunnerMapper : IRunnerMapper
    {
        public Entities.CheckBatch ToEntity(Runner.CheckBatch checkBatch)
        {
            return new Entities.CheckBatch
            {
                Time = checkBatch.Time,
                Duration = checkBatch.Duration
            };
        }

        public Entities.CheckResult ToEntity(Runner.CheckResult checkResult)
        {
            return new Entities.CheckResult
            {
                Type = ToEntity(checkResult.Type),
                Message = checkResult.Message,
                Time = checkResult.Time,
                Duration = checkResult.Duration
            };
        }

        public Entities.CheckResultType ToEntity(Runner.CheckResultType type)
        {
            switch (type)
            {
                case Runner.CheckResultType.Success:
                    return Entities.CheckResultType.Success;

                case Runner.CheckResultType.Failure:
                    return Entities.CheckResultType.Failure;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
