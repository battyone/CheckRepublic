using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public interface ICheckRunner
    {
        Task<CheckResult> ExecuteCheckAsync(ICheck check, CancellationToken token);
    }
}