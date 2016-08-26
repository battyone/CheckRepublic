using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public interface ICheck
    {
        string Name { get; }
        Task<CheckResultData> ExecuteAsync(CancellationToken token);
    }
}
