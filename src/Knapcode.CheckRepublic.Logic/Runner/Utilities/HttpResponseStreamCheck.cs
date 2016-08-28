using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public class HttpResponseStreamCheck : IHttpResponseStreamCheck
    {
        private readonly HttpClient _httpClient;

        public HttpResponseStreamCheck()
        {
            var httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true
            };

            _httpClient = new HttpClient(httpClientHandler)
            {
                Timeout = TimeSpan.FromMinutes(1)
            };
        }

        public async Task<CheckResultData> ExecuteAsync(
            string url,
            Func<Stream, CancellationToken, Task<CheckResultData>> processAsync,
            CancellationToken token)
        {
            using (var response = await _httpClient.GetAsync(url, token))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new CheckResultData
                    {
                        Type = CheckResultType.Failure,
                        Message = $"URL: {url}{Environment.NewLine}Response status was '{(int)response.StatusCode} {response.ReasonPhrase}'."
                    };
                }

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    return await processAsync(stream, token);
                }
            }
        }
    }
}
