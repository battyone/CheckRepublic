using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckNotificationService : ICheckNotificationService
    {
        private const int CountThreshold = 3;
        private static readonly TimeSpan DurationThreshold = TimeSpan.FromMinutes(15);

        private readonly CheckContext _context;

        public CheckNotificationService(CheckContext context)
        {
            _context = context;
        }

        public async Task<CheckNotification> CheckForNotificationAsync(string checkName, CancellationToken token)
        {
            var newestFailure = await _context
                .CheckResults
                .Where(x => x.Check.Name == checkName)
                .OrderByDescending(x => x.Time)
                .FirstAsync();

            if (newestFailure.Type != CheckResultType.Failure)
            {
                // The check is not currently failing.
                return null;
            }

            var timeThreshold = DateTimeOffset.UtcNow - DurationThreshold;

            var oldestFailures = await _context
                .CheckResults
                .Where(x => x.Check.Name == checkName &&
                            x.Time > timeThreshold &&
                            x.Type == CheckResultType.Failure)
                .OrderBy(x => x.Time)
                .Take(CountThreshold)
                .ToListAsync();

            if (oldestFailures.Count < CountThreshold)
            {
                // Not enough failures.
                return null;
            }

            var oldestFailure = oldestFailures.First();

            var notification = await _context
                .CheckNotifications
                .Where(x => x.CheckId == oldestFailure.CheckId)
                .FirstOrDefaultAsync();
            
            if (notification == null)
            {
                notification = new CheckNotification
                {
                    CheckId = oldestFailure.CheckId,
                    Version = 0
                };

                _context.CheckNotifications.Add(notification);
            }
            else if (notification.CheckResultId == oldestFailure.CheckResultId)
            {
                // This check result has already been notified.
                return null;
            }

            notification.CheckResultId = oldestFailure.CheckResultId;
            notification.CheckResult = oldestFailure;
            notification.Time = DateTimeOffset.UtcNow;
            notification.Version++;

            // Add the notification record
            var record = new CheckNotificationRecord
            {
                CheckId = notification.CheckId,
                CheckResultId = notification.CheckResultId,
                Time = notification.Time,
                Version = notification.Version,
                CheckNotification = notification
            };

            _context.CheckNotificationRecords.Add(record);

            try
            {
                await _context.SaveChangesAsync();
                return notification;
            }
            catch (DbUpdateConcurrencyException)
            {
                // This means another caller updated the notification already.
                return null;
            }
        }
    }
}
