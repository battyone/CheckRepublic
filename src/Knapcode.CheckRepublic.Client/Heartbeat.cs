using System;

namespace Knapcode.CheckRepublic.Client
{
    public class Heartbeat
    {
        public long HeartbeatId { get; set; }
        public int HeartId { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
