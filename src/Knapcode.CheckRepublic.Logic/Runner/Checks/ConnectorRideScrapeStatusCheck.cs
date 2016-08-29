using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class ConnectorRideScrapeStatusCheck : ICheck
    {
        private static readonly TimeSpan ErrorThreshold = TimeSpan.FromDays(1);
        private const string SchedulesUrl = "https://connectorride.blob.core.windows.net/scrape/schedules/latest-status.json";
        private const string GtfsUrl = "https://connectorride.blob.core.windows.net/scrape/gtfs/latest-status.json";
        private const string GtfsUngroupedUrl = "https://connectorride.blob.core.windows.net/scrape/gtfs-ungrouped/latest-status.json";

        private readonly IHttpJTokenCheck _check;

        public ConnectorRideScrapeStatusCheck(IHttpJTokenCheck check)
        {
            _check = check;
        }

        public string Name => "Connector Ride Scrape Status";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            {
                var schedulesResult = await CheckStatusAsync(SchedulesUrl, token);
                if (schedulesResult.Type != CheckResultType.Success)
                {
                    return schedulesResult;
                }
            }

            {
                var gtfsResult = await CheckStatusAsync(GtfsUrl, token);
                if (gtfsResult.Type != CheckResultType.Success)
                {
                    return gtfsResult;
                }
            }

            {
                var gtfsUngroupedResult = await CheckStatusAsync(GtfsUngroupedUrl, token);
                if (gtfsUngroupedResult.Type != CheckResultType.Success)
                {
                    return gtfsUngroupedResult;
                }
            }

            return new CheckResultData
            {
                Type = CheckResultType.Success
            };
        }

        private async Task<CheckResultData> CheckStatusAsync(string url, CancellationToken token)
        {
            var now = DateTimeOffset.UtcNow;

            return await _check.ExecuteAsync(
                url,
                jToken =>
                {
                    var lastUploaded = jToken.Value<DateTimeOffset>("Time");
                    var sinceLastUploaded = now - lastUploaded;

                    if (sinceLastUploaded > ErrorThreshold)
                    {
                        return new CheckResultData
                        {
                            Type = CheckResultType.Failure,
                            Message = $"The last upload was at {lastUploaded} ({sinceLastUploaded} ago) which is greater than the error threshold, {ErrorThreshold}."
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
