using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public class CheckRunnerService : ICheckRunnerService
    {
        private readonly ICheckFactory _factory;
        private readonly ICheckPersister _persister;
        private readonly ICheckBatchRunner _batchRunner;

        public CheckRunnerService(ICheckBatchRunner batchRunner, ICheckPersister persister, ICheckFactory factory)
        {
            _batchRunner = batchRunner;
            _persister = persister;
            _factory = factory;
        }

        public async Task<Entities.CheckBatch> RunAsync(CancellationToken token)
        {
            var checks = _factory.BuildAll();

            var runnerBatch = await _batchRunner.ExecuteAsync(checks, token);

            var batch = await _persister.PersistBatchAsync(runnerBatch, token);

            return batch;
        }
    }
}
