using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class Check
    {
        public int CheckId { get; set; }
        public string Name { get; set; }

        public List<CheckResult> CheckResults { get; set; }
    }
}
