using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public class HttpJTokenCheck : IHttpJTokenCheck
    {
        private readonly IHttpResponseStreamCheck _check;

        public HttpJTokenCheck(IHttpResponseStreamCheck check)
        {
            _check = check;
        }

        public async Task<CheckResultData> ExecuteAsync(string url, Func<JToken, CheckResultData> process, CancellationToken token)
        {
            return await _check.ExecuteAsync(
                url,
                (stream, innerToken) =>
                {
                    using (var streamReader = new StreamReader(stream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        jsonTextReader.DateParseHandling = DateParseHandling.DateTimeOffset;

                        var jToken = JToken.Load(jsonTextReader);

                        var result = process(jToken);

                        return Task.FromResult(result);
                    }
                },
                token);
        }
    }
}
