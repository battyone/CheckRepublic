using Knapcode.CheckRepublic.Logic.Checks;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckResultEntity
    {
        public long Id { get; set; }
        public CheckResultType Type { get; set; }
        public string Message { get; set; }
    }
}
