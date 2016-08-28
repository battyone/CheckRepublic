using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class Heart
    {
        public int HeartId { get; set; }
        public int HeartGroupId { get; set; }
        public string Name { get; set; }

        public HeartGroup HeartGroup { get; set; }
        public List<Heartbeat> Heartbeats { get; set; }
    }
}
