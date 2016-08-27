using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner;
using Knapcode.CheckRepublic.Logic.Runner.Checks;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckRunnerService : ICheckRunnerService
    {
        private readonly ICheckPersister _persister;
        private readonly ICheckBatchRunner _runner;

        public CheckRunnerService(ICheckBatchRunner runner, ICheckPersister persister)
        {
            _runner = runner;
            _persister = persister;
        }

        public async Task<Entities.CheckBatch> RunAsync(CancellationToken token)
        {
            var checks = new[]
            {
                new NuGetToolsCheck()
            };

            var runnerBatch = await _runner.ExecuteAsync(checks, token);

            var batch = await _persister.PersistBatchAsync(runnerBatch, token);

            return batch;
        }
    }
}
