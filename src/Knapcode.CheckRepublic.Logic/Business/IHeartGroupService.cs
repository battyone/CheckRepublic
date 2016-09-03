using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface IHeartGroupService
    {
        Task<IEnumerable<Heartbeat>> GetHeartbeatsByHeartGroupName(string heartGroupName, int skip, int take, bool asc, CancellationToken token);
        Task<IEnumerable<HeartGroup>> GetHeartGroupsAsync(int skip, int take, bool asc, CancellationToken token);
    }
}