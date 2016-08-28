using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public interface IHttpCheck
    {
        Task<CheckResultData> ExecuteAsync(string url, string substring, CancellationToken token);
    }
}