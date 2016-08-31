using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class NotificationCheckService : INotificationCheckService
    {
        private const int CountThreshold = 3;
        private static readonly TimeSpan DurationThreshold = TimeSpan.FromDays(15);

        private readonly CheckContext _context;

        public NotificationCheckService(CheckContext context)
        {
            _context = context;
        }

        public async Task<CheckNotification> CheckForNotificationAsync(string checkName, CancellationToken token)
        {
            var timeThreshold = DateTimeOffset.UtcNow - DurationThreshold;

            var failures = await _context
                .CheckResults
                .Where(x => x.Check.Name == checkName &&
                            x.Time > timeThreshold &&
                            x.Type == CheckResultType.Failure)
                .OrderByDescending(x => x.Time)
                .Take(CountThreshold)
                .ToListAsync();

            if (failures.Count < CountThreshold)
            {
                // Not enough failures.
                return null;
            }

            var oldestFailure = failures.Last();

            var notification = await _context
                .CheckNotifications
                .Where(x => x.CheckId == oldestFailure.CheckId)
                .FirstOrDefaultAsync();

            if (notification.CheckResultId == oldestFailure.CheckResultId)
            {
                // This check result has already been notified.
                return null;
            }

            if (notification == null)
            {
                notification = new CheckNotification
                {
                    CheckId = oldestFailure.CheckId
                };

                _context.CheckNotifications.Add(notification);
            }

            notification.CheckResultId = oldestFailure.CheckResultId;
            notification.Time = DateTimeOffset.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return notification;
            }
            catch (DbUpdateConcurrencyException)
            {
                // This means another caller updated the record already.
                return null;
            }
        }
    }
}
