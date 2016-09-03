using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class LoggerNotificationSender : INotificationSender
    {
        private readonly ILogger<LoggerNotificationSender> _logger;

        public LoggerNotificationSender(ILogger<LoggerNotificationSender> logger)
        {
            _logger = logger;
        }

        public Task SendNotificationAsync(string text, CancellationToken token)
        {
            _logger.LogWarning(text);

            return Task.CompletedTask;
        }
    }
}
