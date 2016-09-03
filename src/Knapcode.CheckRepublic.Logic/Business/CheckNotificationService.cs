using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.CheckRepublic.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckNotificationService : ICheckNotificationService
    {
        private const int CountThreshold = 3;
        private static readonly TimeSpan DurationThreshold = TimeSpan.FromMinutes(15);

        private readonly Entities.CheckContext _context;
        private readonly ISystemClock _systemClock;
        private readonly IEntityMapper _entityMapper;

        public CheckNotificationService(ISystemClock systemClock, Entities.CheckContext context, IEntityMapper entityMapper)
        {
            _systemClock = systemClock;
            _context = context;
            _entityMapper = entityMapper;
        }

        public async Task<CheckNotification> CheckForNotificationAsync(string checkName, CancellationToken token)
        {
            var checkResultAndHealth = await GetCheckResultAndHealthAsync(checkName, token);
            if (checkResultAndHealth == null)
            {
                // Don't notify when there is no check result to act on.
                return null;
            }

            var notification = await GetNotificationAsync(checkName, token);
            if (notification == null)
            {
                if (checkResultAndHealth.IsHealthy)
                {
                    // Don't notify when the check is healthy and there is no previous notification.
                    return null;
                }

                notification = new Entities.CheckNotification
                {
                    CheckId = checkResultAndHealth.CheckResult.CheckId,
                    Version = 0
                };

                _context.CheckNotifications.Add(notification);
            }
            else if (notification.IsHealthy == checkResultAndHealth.IsHealthy)
            {
                // Don't notify the health status is not changing.
                return null;
            }

            notification.CheckResultId = checkResultAndHealth.CheckResult.CheckResultId;
            notification.CheckResult = checkResultAndHealth.CheckResult;
            notification.Time = _systemClock.UtcNow;
            notification.IsHealthy = checkResultAndHealth.IsHealthy;
            notification.Version++;

            // Add the notification record
            var record = new Entities.CheckNotificationRecord
            {
                CheckId = notification.CheckId,
                CheckResultId = notification.CheckResultId,
                Time = notification.Time,
                IsHealthy = notification.IsHealthy,
                Version = notification.Version,
                CheckNotification = notification
            };

            _context.CheckNotificationRecords.Add(record);

            try
            {
                await _context.SaveChangesAsync();
                return _entityMapper.ToBusiness(notification);
            }
            catch (DbUpdateConcurrencyException)
            {
                // This means another caller updated the notification already.
                return null;
            }
        }

        private async Task<Entities.CheckNotification> GetNotificationAsync(string checkName, CancellationToken token)
        {
            var notification = await _context
                .CheckNotifications
                .Where(x => x.Check.Name == checkName)
                .Include(x => x.CheckResult)
                .FirstOrDefaultAsync();

            return notification;
        }

        private async Task<CheckResultAndHealth> GetCheckResultAndHealthAsync(string checkName, CancellationToken token)
        {
            var latestCheckResult = await _context
                .CheckResults
                .Where(x => x.Check.Name == checkName)
                .OrderByDescending(x => x.Time)
                .FirstOrDefaultAsync();

            if (latestCheckResult == null)
            {
                // No check results exist.
                return null;
            }

            if (latestCheckResult.Type != Entities.CheckResultType.Failure)
            {
                // Not currently failing.
                return new CheckResultAndHealth
                {
                    CheckResult = latestCheckResult,
                    IsHealthy = true
                };
            }

            var timeThreshold = _systemClock.UtcNow - DurationThreshold;

            var oldestFailures = await _context
                .CheckResults
                .Where(x => x.Check.Name == checkName &&
                            x.Time > timeThreshold &&
                            x.Type == Entities.CheckResultType.Failure)
                .OrderBy(x => x.Time)
                .Take(CountThreshold)
                .ToListAsync();

            if (oldestFailures.Count < CountThreshold)
            {
                // Not enough failures to act on.
                return null;
            }

            var oldestFailure = oldestFailures.First();

            return new CheckResultAndHealth
            {
                CheckResult = oldestFailure,
                IsHealthy = false
            };
        }

        private class CheckResultAndHealth
        {
            public Entities.CheckResult CheckResult { get; set; }
            public bool IsHealthy { get; set; }
        }
    }
}
