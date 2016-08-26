using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public interface ICheckRunner
    {
        Task<CheckResult> ExecuteAsync(ICheck check, CancellationToken token);
    }
}