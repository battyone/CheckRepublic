using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public interface IHttpJTokenCheck
    {
        Task<CheckResultData> ExecuteAsync(string url, Func<JToken, CheckResultData> process, CancellationToken token);
    }
}