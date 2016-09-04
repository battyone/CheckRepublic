using System;
using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckBatch
    {
        public long CheckBatchId { get; set; }
        public DateTimeOffset TimeText { get; set; }
        public TimeSpan DurationText { get; set; }
        public long Time { get; set; }
        public long Duration { get; set; }

        public List<CheckResult> CheckResults { get; set; }
    }
}
