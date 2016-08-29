using System;

namespace Knapcode.CheckRepublic.Logic.Business.Models
{
    public class RunnerHealth
    {
        public bool IsHealthy { get; set; }
        public string Message { get; set; }
        public DateTimeOffset LastRunTime { get; set; }
    }
}
