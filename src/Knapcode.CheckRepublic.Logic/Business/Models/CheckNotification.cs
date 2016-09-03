using System;

namespace Knapcode.CheckRepublic.Logic.Business.Models
{
    public class CheckNotification
    {
        public int CheckNotificationId { get; set; }
        public int Version { get; set; }
        public int CheckId { get; set; }
        public long CheckResultId { get; set; }
        public DateTimeOffset Time { get; set; }
        public bool IsHealthy { get; set; }

        public CheckResult CheckResult { get; set; }
    }
}
