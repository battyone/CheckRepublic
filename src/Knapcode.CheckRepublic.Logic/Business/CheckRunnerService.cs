using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business.Models;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckRunnerService : ICheckRunnerService
    {
        private readonly Runner.ICheckFactory _factory;
        private readonly ICheckPersister _persister;
        private readonly Runner.ICheckBatchRunner _batchRunner;

        public CheckRunnerService(Runner.ICheckBatchRunner batchRunner, ICheckPersister persister, Runner.ICheckFactory factory)
        {
            _batchRunner = batchRunner;
            _persister = persister;
            _factory = factory;
        }

        public async Task<CheckBatch> RunAsync(CancellationToken token)
        {
            var checks = _factory.BuildAll();

            var runnerBatch = await _batchRunner.ExecuteAsync(checks, token);

            var batch = await _persister.PersistBatchAsync(runnerBatch, token);

            return batch;
        }
    }
}
