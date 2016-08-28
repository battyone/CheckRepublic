using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface ICheckBatchService
    {
        Task<IEnumerable<CheckBatch>> GetCheckBatchesAsync(int skip, int take, bool asc, CancellationToken token);
        Task<CheckBatch> GetCheckBatchByIdAsync(int id, CancellationToken token);
        Task<CheckBatch> GetLatestCheckBatchAsync(CancellationToken token);
    }
}