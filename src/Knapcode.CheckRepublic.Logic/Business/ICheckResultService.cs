using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface ICheckResultService
    {
        Task<IEnumerable<CheckResult>> GetFailedCheckResultsAsync(int skip, int take, bool asc, CancellationToken token);
    }
}