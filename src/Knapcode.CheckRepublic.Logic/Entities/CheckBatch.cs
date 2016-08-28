using System;
using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckBatch
    {
        public long CheckBatchId { get; set; }
        public string MachineName { get; set; }
        public DateTimeOffset Time { get; set; }
        public TimeSpan Duration { get; set; }

        public List<CheckResult> CheckResults { get; set; }
    }
}
