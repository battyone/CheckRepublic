using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public class IsNuGetToolsUp : ICheck
    {
        private const string RequestUri = "http://nugettoolsdev.azurewebsites.net/3.5.0-rc1-final/parse-framework?framework=.netframework%2Cversion%3Dv4.0";
        private const string ExpectedSubstring = ".NETFramework,Version=v4.0";

        private readonly HttpClient _httpClient;

        public IsNuGetToolsUp()
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

        public async Task<CheckResult> ExecuteAsync(CancellationToken token)
        {
            var response = await _httpClient.GetAsync(RequestUri, token);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new CheckResult
                {
                    Type = CheckResultType.Failure,
                    Message = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}"
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (!content.Contains(ExpectedSubstring))
            {
                return new CheckResult
                {
                    Type = CheckResultType.Failure,
                    Message = $"The response body did not contain '{ExpectedSubstring}'."
                };
            }

            return new CheckResult
            {
                Type = CheckResultType.Success
            };
        }
    }
}
