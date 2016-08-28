using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class HeartGroup
    {
        public int HeartGroupId { get; set; }
        public string Name { get; set; }

        public List<Heart> Hearts { get; set; }
    }
}
