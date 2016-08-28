using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public interface IHttpResponseStreamCheck
    {
        Task<CheckResultData> ExecuteAsync(string url, Func<Stream, CancellationToken, Task<CheckResultData>> processAsync, CancellationToken token);
    }
}