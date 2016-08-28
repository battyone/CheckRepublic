using System;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public interface IHeartbeatCheck
    {
        Task<CheckResultData> ExecuteAsync(string heartGroupName, TimeSpan errorThreshold, CancellationToken token);
    }
}