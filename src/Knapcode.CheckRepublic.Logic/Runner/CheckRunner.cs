using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public class CheckRunner : ICheckRunner
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(1);
        private readonly ISystemClock _systemClock;

        public CheckRunner(ISystemClock systemClock)
        {
            _systemClock = systemClock;
        }

        public async Task<CheckResult> ExecuteAsync(ICheck check, CancellationToken token)
        {
            using (var timeoutTcs = new CancellationTokenSource())
            using (var taskTcs = new CancellationTokenSource())
            using (token.Register(() => taskTcs.Cancel()))
            {
                var timeoutTask = Task.Delay(Timeout, timeoutTcs.Token);

                var now = _systemClock.UtcNow;

                var stopwatch = Stopwatch.StartNew();
                var checkTask = check.ExecuteAsync(taskTcs.Token);

                if (timeoutTask == await Task.WhenAny(checkTask, timeoutTask))
                {
                    taskTcs.Cancel();

                    return new CheckResult
                    {
                        Check = check,
                        Type = CheckResultType.Failure,
                        Message = "The check timed out.",
                        Time = now,
                        Duration = stopwatch.Elapsed
                    };
                }

                timeoutTcs.Cancel();
                
                try
                {
                    var data = await checkTask;
                    return new CheckResult
                    {
                        Check = check,
                        Type = data.Type,
                        Message = data.Message,
                        Time = now,
                        Duration = stopwatch.Elapsed
                    };
                }
                catch (Exception exception)
                {
                    return new CheckResult
                    {
                        Check = check,
                        Type = CheckResultType.Failure,
                        Message = ExceptionUtility.GetDisplayMessage(exception),
                        Time = now,
                        Duration = stopwatch.Elapsed
                    };
                }
            }
        }
    }
}
