using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface ICheckPersister
    {
        Task PersistBatchAsync(CheckBatch batch, CancellationToken token);
    }
}