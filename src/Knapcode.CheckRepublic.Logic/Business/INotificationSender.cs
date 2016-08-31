using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(string text, CancellationToken token);
    }
}