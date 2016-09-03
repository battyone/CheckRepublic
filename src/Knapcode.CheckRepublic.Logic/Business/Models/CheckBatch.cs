using System;
using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Business.Models
{
    public class CheckBatch
    {
        public long CheckBatchId { get; set; }
        public DateTimeOffset Time { get; set; }
        public TimeSpan Duration { get; set; }

        public List<CheckResult> CheckResults { get; set; }
    }
}
