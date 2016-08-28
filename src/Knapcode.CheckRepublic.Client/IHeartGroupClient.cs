using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Client
{
    public interface IHeartGroupClient
    {
        Task<Heartbeat> CreateHeartbeatAsync(string heartGroupName, string heartName, CancellationToken token);
    }
}