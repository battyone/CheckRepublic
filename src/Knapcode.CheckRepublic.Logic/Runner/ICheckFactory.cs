using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public interface ICheckFactory
    {
        IEnumerable<ICheck> BuildAll();
    }
}