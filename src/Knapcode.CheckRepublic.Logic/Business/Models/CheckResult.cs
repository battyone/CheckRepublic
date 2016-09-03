using System;

namespace Knapcode.CheckRepublic.Logic.Business.Models
{
    public class CheckResult
    {
        public long CheckResultId { get; set; }
        public long CheckBatchId { get; set; }
        public int CheckId { get; set; }
        public CheckResultType Type { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Time { get; set; }
        public TimeSpan Duration { get; set; }

        public Check Check { get; set; }
    }
}
