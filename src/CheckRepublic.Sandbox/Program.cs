using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Runner;
using Knapcode.CheckRepublic.Logic.Runner.Checks;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            var dbContextFactoryOptions = new DbContextFactoryOptions();

            using (var context = checkContextFactory.Create(dbContextFactoryOptions))
            {
                var runner = new CheckRunner();
                var batchRunner = new CheckBatchRunner(runner);
                var persister = new CheckPersister(context);

                var nuGetToolsCheck = new NuGetToolsCheck();
                var checks = new[] { nuGetToolsCheck };
                var batch = await batchRunner.ExecuteAsync(checks, token);

                await persister.PersistBatchAsync(batch, token);
            }
        }
    }
}
