using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface INotificationRunnerService
    {
        Task<IEnumerable<CheckNotification>> RunAsync(CancellationToken token);
    }
}