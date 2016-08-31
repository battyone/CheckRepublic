using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Knapcode.CheckRepublic.Sandbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args, CancellationToken.None).Wait();
        }

        public static async Task MainAsync(string[] args, CancellationToken token)
        {
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

                var websiteTask = Task.Run(() => Website.Program.Main(new string[0]));

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "OldyaXRl");
                httpClient.Timeout = Timeout.InfiniteTimeSpan;

                var heartGroupClient = new HeartGroupClient("http://localhost:5000", "Write");
                var heartbeat = await heartGroupClient.CreateHeartbeatAsync("PoGoNotifications.PokemonEncounter", Environment.MachineName, token);
                Console.WriteLine(Serialize(heartbeat));

                var checkResponse = await httpClient.PostAsync("http://localhost:5000/api/checkrunner", new ByteArrayContent(new byte[0]));
                var checkResponseString = await checkResponse.Content.ReadAsStringAsync();
                Console.WriteLine(Serialize(JsonConvert.DeserializeObject(checkResponseString)));
                
                var notificationResponse = await httpClient.PostAsync("http://localhost:5000/api/notificationrunner", new ByteArrayContent(new byte[0]));
                var notificationResponseString = await notificationResponse.Content.ReadAsStringAsync();
                Console.WriteLine(Serialize(JsonConvert.DeserializeObject(notificationResponseString)));

                await websiteTask;
            }
        }

        private static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                Converters =
                {
                    new StringEnumConverter()
                },
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
