using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public class CheckBatchRunner : ICheckBatchRunner
    {
        private const int MaxDegreeOfParallelism = 8;
        private readonly ICheckRunner _checkRunner;

        public CheckBatchRunner(ICheckRunner checkRunner)
        {
            _checkRunner = checkRunner;
        }

        public async Task<CheckBatch> ExecuteAsync(IEnumerable<ICheck> checks, CancellationToken token)
        {
            var time = DateTimeOffset.UtcNow;
            var stopwatch = Stopwatch.StartNew();

            var checkBag = new ConcurrentBag<ICheck>(checks);
            var resultBag = new ConcurrentBag<CheckResult>();

            var tasks = Enumerable
                .Range(0, MaxDegreeOfParallelism)
                .Select(i => ProcessChecksAsync(checkBag, resultBag, token))
                .ToArray();

            await Task.WhenAll(tasks);

            var results = resultBag
                .OrderBy(x => x.Time)
                .ToList();

            return new CheckBatch
            {
                MachineName = Environment.MachineName,
                Time = time,
                Duration = stopwatch.Elapsed,
                CheckResults = results
            };
        }

        private async Task ProcessChecksAsync(ConcurrentBag<ICheck> checks, ConcurrentBag<CheckResult> results, CancellationToken token)
        {
            ICheck check;
            while (checks.TryTake(out check))
            {
                var result = await _checkRunner.ExecuteAsync(check, token);
                results.Add(result);
            }
        }
    }
}
