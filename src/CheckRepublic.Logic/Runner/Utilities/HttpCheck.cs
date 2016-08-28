using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Runner.Utilities
{
    public class HttpCheck : IHttpCheck
    {
        private readonly HttpClient _httpClient;

        public HttpCheck()
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

        public async Task<CheckResultData> ExecuteAsync(string url, string substring, CancellationToken token)
        {
            var response = await _httpClient.GetAsync(url, token);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new CheckResultData
                {
                    Type = CheckResultType.Failure,
                    Message = $"URL: {url}{Environment.NewLine}Response status was '{(int)response.StatusCode} {response.ReasonPhrase}'."
                };
            }

            var content = await response.Content.ReadAsStringAsync();

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
    }
}
