using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public interface ICheck
    {
        Task<CheckResult> ExecuteAsync(CancellationToken token);
    }
}
