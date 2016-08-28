using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class ConnectorRideLatestJsonCheck : ICheck
    {
        private const string Url = "https://connectorride.blob.core.windows.net/scrape/schedules/latest.json";
        private static readonly TimeSpan ErrorThreshold = TimeSpan.FromDays(1);

        private readonly IHttpJTokenCheck _check;

        public ConnectorRideLatestJsonCheck(IHttpJTokenCheck check)
        {
            _check = check;
        }

        public string Name => "ConnectorRide Latest JSON";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _check.ExecuteAsync(
                Url,
                jToken =>
                {
                    if (jToken["Schedules"].Count() == 0)
                    {
                        return new CheckResultData
                        {
                            Type = CheckResultType.Failure,
                            Message = $"The ConnectorRide JSON does not have any schedules in it."
                        };
                    }

                    return new CheckResultData
                    {
                        Type = CheckResultType.Success
                    };
                },
                token);
        }
    }
}
