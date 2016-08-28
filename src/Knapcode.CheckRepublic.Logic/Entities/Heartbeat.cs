using System;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class Heartbeat
    {
        public long HeartbeatId { get; set; }
        public int HeartId { get; set; }
        public DateTimeOffset Time { get; set; }

        public Heart Heart { get; set; }
    }
}
