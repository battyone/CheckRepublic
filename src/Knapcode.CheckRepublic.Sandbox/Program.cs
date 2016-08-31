using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Client;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
                var checkContextFactory = new CheckContextFactory();
                var dbContextFactoryOptions = new DbContextFactoryOptions();

                using (var context = checkContextFactory.Create(dbContextFactoryOptions))
                {
                    await context.Database.MigrateAsync();

                    var runner = new CheckRunner();
                    var batchRunner = new CheckBatchRunner(runner);
                    var persister = new CheckPersister(context);
                    var factory = new ManualCheckFactory(context);
                    var runnerService = new CheckRunnerService(batchRunner, persister, factory);

                    var batch = await runnerService.RunAsync(token);
                    Console.WriteLine(Serialize(batch));

                    var heartbeatService = new HeartbeatService(context);

                    var heartbeat = await heartbeatService.CreateHeartbeatAsync("PoGoNotifications.PokemonEncounter", Environment.MachineName, token);
                    Console.WriteLine(Serialize(heartbeat));

                    var notificationService = new NotificationCheckService(context);
                    await notificationService.CheckForNotificationAsync("Concerto Up", token);
                }
            }

            {
                var websiteTask = Task.Run(() => Website.Program.Main(new string[0]));

                var heartGroupClient = new HeartGroupClient("http://localhost:5000", "Write");
                var heartbeat = await heartGroupClient.CreateHeartbeatAsync("PoGoNotifications.PokemonEncounter", Environment.MachineName, token);
                Console.WriteLine(Serialize(heartbeat));

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "OldyaXRl");
                var checkResponse = await httpClient.PostAsync("http://localhost:5000/api/checkrunner", new ByteArrayContent(new byte[0]));
                var checkResponseString = await checkResponse.Content.ReadAsStringAsync();
                Console.WriteLine(Serialize(JsonConvert.DeserializeObject(checkResponseString)));

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
