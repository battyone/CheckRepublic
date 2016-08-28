using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Knapcode.CheckRepublic.Client
{
    public class HeartGroupClient : IHeartGroupClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _password;
        private readonly string _url;

        public HeartGroupClient(string url, string password)
        {
            _url = url.Trim('/');
            _password = password;
            _httpClient = new HttpClient();
        }

        public async Task<Heartbeat> CreateHeartbeatAsync(string heartGroupName, string heartName, CancellationToken token)
        {
            var url = $"{_url}/api/heartgroups/name:{Uri.EscapeDataString(heartGroupName)}/hearts/name:{Uri.EscapeDataString(heartName)}/heartbeats";

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Headers.Authorization = GetBasicAuthentication(string.Empty, _password);

                using (var response = await _httpClient.SendAsync(request, token))
                {
                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Heartbeat>(responseString);
                }
            }
        }

        private static AuthenticationHeaderValue GetBasicAuthentication(string username, string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var base64 = Convert.ToBase64String(bytes);

            var header = new AuthenticationHeaderValue("Basic", base64);

            return header;
        }
    }
}
