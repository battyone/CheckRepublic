using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface ICheckPersister
    {
        Task<CheckBatch> PersistBatchAsync(Runner.CheckBatch runnerBatch, CancellationToken token);
    }
}