using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Runner;

namespace Knapcode.CheckRepublic.Logic.Business.Mappers
{
    public interface IRunnerMapper
    {
        Entities.CheckResultType ToEntity(Runner.CheckResultType type);
        Entities.CheckResult ToEntity(Runner.CheckResult checkResult);
        Entities.CheckBatch ToEntity(Runner.CheckBatch checkBatch);
    }
}