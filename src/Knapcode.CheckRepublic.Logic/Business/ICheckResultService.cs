using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface ICheckResultService
    {
        Task<IEnumerable<CheckResult>> GetCheckResultsByCheckIdAsync(int checkId, int skip, int take, bool asc, CancellationToken token);
        Task<IEnumerable<CheckResult>> GetCheckResultsByCheckNameAsync(string checkName, int skip, int take, bool asc, CancellationToken token);
        Task<IEnumerable<CheckResult>> GetFailureCheckResultsAsync(int skip, int take, bool asc, CancellationToken token);
        Task<IEnumerable<CheckResult>> GetFailureCheckResultsByCheckIdAsync(int checkId, int skip, int take, bool asc, CancellationToken token);
        Task<IEnumerable<CheckResult>> GetFailureCheckResultsByCheckNameAsync(string checkName, int skip, int take, bool asc, CancellationToken token);
    }
}