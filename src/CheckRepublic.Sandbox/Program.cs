using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Checks;
using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;

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
            var checkContextFactory = new CheckContextFactory();

            var checkRunner = new CheckRunner();
            var checkBatchRunner = new CheckBatchRunner(checkRunner);

            var check = new NuGetToolsCheck();
            var checkBatch = await checkBatchRunner.ExecuteAsync(new[] { check }, token);

            var output = JsonConvert.SerializeObject(checkBatch);
            Console.WriteLine(output);
        }
    }
}
