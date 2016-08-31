using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface INotificationCheckService
    {
        Task<CheckNotification> CheckForNotificationAsync(string checkName, CancellationToken token);
    }
}