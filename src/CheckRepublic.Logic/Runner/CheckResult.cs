using System;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public class CheckResult
    {
        public ICheck Check { get; set; }
        public CheckResultType Type { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Time { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
