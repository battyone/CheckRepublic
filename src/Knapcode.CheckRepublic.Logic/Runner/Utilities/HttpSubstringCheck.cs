using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public class HttpSubstringCheck : IHttpSubstringCheck
    {
        private readonly IHttpResponseStreamCheck _check;

        public HttpSubstringCheck(IHttpResponseStreamCheck check)
        {
            _check = check;
        }

        public async Task<CheckResultData> ExecuteAsync(string url, string substring, CancellationToken token)
        {
            return await _check.ExecuteAsync(
                url,
                async (stream, innerToken) =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var content = await reader.ReadToEndAsync();

                        if (!content.Contains(substring))
                        {
                            return new CheckResultData
                            {
                                Type = CheckResultType.Failure,
                                Message = $"URL: {url}{Environment.NewLine}Response body did not contain '{substring}'."
                            };
                        }

                        return new CheckResultData
                        {
                            Type = CheckResultType.Success
                        };
                    }
                },
                token);
        }
    }
}
