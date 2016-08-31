using System;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckNotificationRecord
    {
        public int CheckNotificationId { get; set; }
        public int Version { get; set; }
        public int CheckId { get; set; }
        public long CheckResultId { get; set; }
        public DateTimeOffset Time { get; set; }
         
        public CheckNotification CheckNotification { get; set; }
        public Check Check { get; set; }
        public CheckResult CheckResult { get; set; }
    }
}
