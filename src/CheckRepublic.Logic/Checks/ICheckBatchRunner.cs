using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public interface ICheckBatchRunner
    {
        Task<CheckBatch> ExecuteAsync(IEnumerable<ICheck> checks, CancellationToken token);
    }
}