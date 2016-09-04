using System;
using System.Linq;
using Knapcode.CheckRepublic.Logic.Utilities;

namespace Knapcode.CheckRepublic.Logic.Entities.DataMigrations
{
    public class AddIntegerTimeAndDurationColumnsDataMigration : IDataMigration
    {
        private const int Take = 5000;

        public void Up(CheckContext context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                Apply(
                    context,
                    x => x.CheckBatches.OrderBy(e => e.CheckBatchId),
                    x =>
                    {
                        x.Time = TimeUtilities.DateTimeOffsetToLong(x.TimeText);
                        x.Duration = TimeUtilities.TimeSpanToLong(x.DurationText);
                    });

                Apply(
                    context,
                    x => x.CheckNotifications.OrderBy(e => e.CheckNotificationId),
                    x =>
                    {
                        x.Time = TimeUtilities.DateTimeOffsetToLong(x.TimeText);
                    });

                Apply(
                    context,
                    x => x.CheckNotificationRecords.OrderBy(e => e.CheckNotificationId).ThenBy(e => e.Version),
                    x =>
                    {
                        x.Time = TimeUtilities.DateTimeOffsetToLong(x.TimeText);
                    });

                Apply(
                    context,
                    x => x.CheckResults.OrderBy(e => e.CheckResultId),
                    x =>
                    {
                        x.Time = TimeUtilities.DateTimeOffsetToLong(x.TimeText);
                        x.Duration = TimeUtilities.TimeSpanToLong(x.DurationText);
                    });

                Apply(
                    context,
                    x => x.Heartbeats.OrderBy(e => e.HeartbeatId),
                    x =>
                    {
                        x.Time = TimeUtilities.DateTimeOffsetToLong(x.TimeText);
                    });

                transaction.Commit();
            }
        }

        private void Apply<T>(CheckContext context, Func<CheckContext, IQueryable<T>> getOrderedEntities, Action<T> updateEntity)
        {
            var skip = 0;
            var lastCount = Take;
            while (lastCount >= Take)
            {
                var batch = getOrderedEntities(context)
                        .Skip(skip)
                        .Take(Take);

                lastCount = 0;
                foreach (var entity in batch)
                {
                    updateEntity(entity);
                    lastCount++;
                }

                skip += lastCount;
                context.SaveChanges();
            }
        }
    }
}
