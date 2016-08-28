using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface IHeartbeatService
    {
        Task<Heartbeat> CreateHeartbeatAsync(string heartGroupName, string heartName, CancellationToken token);
    }
}