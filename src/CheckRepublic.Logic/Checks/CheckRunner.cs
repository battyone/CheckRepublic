using System;
using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.CheckRepublic.Logic.Checks
{
    public class CheckRunner : ICheckRunner
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(1);

        public async Task<CheckResult> ExecuteCheckAsync(ICheck check, CancellationToken token)
        {
            using (var timeoutTcs = new CancellationTokenSource())
            using (var taskTcs = new CancellationTokenSource())
            using (token.Register(() => taskTcs.Cancel()))
            {
                var timeoutTask = Task.Delay(Timeout, timeoutTcs.Token);

                var checkTask = check.ExecuteAsync(taskTcs.Token);

                if (timeoutTask == await Task.WhenAny(checkTask, timeoutTask))
                {
                    taskTcs.Cancel();

                    return new CheckResult
                    {
                        Type = CheckResultType.Failure,
                        Message = "The check timed out."
                    };
                }

                timeoutTcs.Cancel();

                return await checkTask;
            }
        }
    }
}
