using System;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckNotification
    {
        public int CheckNotificationId { get; set; }
        public int Version { get; set; }
        public int CheckId { get; set; }
        public long CheckResultId { get; set; }
        public DateTimeOffset Time { get; set; }
        public bool IsHealthy { get; set; }
         
        public Check Check { get; set; }
        public CheckResult CheckResult { get; set; }
    }
}
