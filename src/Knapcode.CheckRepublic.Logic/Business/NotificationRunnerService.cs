using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class NotificationRunnerService : INotificationRunnerService
    {
        private const int Take = 100;

        private readonly ICheckNotificationService _checkNotificationService;
        private readonly ICheckService _checkService;
        private readonly INotificationSender _sender;

        public NotificationRunnerService(
            ICheckService checkService,
            ICheckNotificationService checkNotificationService,
            INotificationSender sender)
        {
            _checkService = checkService;
            _checkNotificationService = checkNotificationService;
            _sender = sender;
        }

        public async Task<IEnumerable<CheckNotification>> RunAsync(CancellationToken token)
        {
            var notifications = new List<CheckNotification>();
            var skip = 0;
            var pageSize = Take;
            while (pageSize >= Take)
            {
                var checks = await _checkService.GetChecksAsync(skip, Take, asc: true, token: token);
                pageSize = checks.Count();
                skip += pageSize;

                foreach (var check in checks)
                {
                    var notification = await _checkNotificationService.CheckForNotificationAsync(check.Name, token);

                    if (notification != null)
                    {
                        await SendAsync(check, notification, token);

                        notifications.Add(notification);
                    }
                }
            }

            return notifications;
        }

        private async Task SendAsync(Check check, CheckNotification notification, CancellationToken token)
        {
            string text;
            if (notification.IsHealthy)
            {
                text = $"The check '{check.Name}' has been HEALTHY since {notification.CheckResult.Time.ToLocalTime()}.";
            }
            else
            {
                text = $"The check '{check.Name}' has been FAILING since {notification.CheckResult.Time.ToLocalTime()}.";
            }

            await _sender.SendNotificationAsync(text, token);
        }
    }
}
