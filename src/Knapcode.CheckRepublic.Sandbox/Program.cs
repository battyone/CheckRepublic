using System;
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
                    var factory = new ManualCheckFactory();
                    var runnerService = new CheckRunnerService(batchRunner, persister, factory);

                    var batch = await runnerService.RunAsync(token);
                    Console.WriteLine(Serialize(batch));

                    var heartbeatService = new HeartbeatService(context);

                    var heartbeat = await heartbeatService.CreateHeartbeatAsync("Sandbox", "Sandbox", token);
                    Console.WriteLine(Serialize(heartbeat));
                }
            }

            {

                var websiteTask = Task.Run(() => Website.Program.Main(new string[0]));

                var client = new HeartGroupClient("http://localhost:5000", "Write");
                var heartbeat = await client.CreateHeartbeatAsync("Sandbox", "Sandbox", token);
                Console.WriteLine(Serialize(heartbeat));

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
