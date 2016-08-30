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
                var result = await CheckStatusAsync(SchedulesUrl, token);
                if (result.Type != CheckResultType.Success)
                {
                    return result;
                }
            }

            {
                var result = await CheckStatusAsync(GtfsUrl, token);
                if (result.Type != CheckResultType.Success)
                {
                    return result;
                }
            }

            {
                var result = await CheckStatusAsync(GtfsUngroupedUrl, token);
                if (result.Type != CheckResultType.Success)
                {
                    return result;
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
                    var lastUploadedString = jToken.Value<string>("Time");
                    var lastUploaded = DateTimeOffset.Parse(lastUploadedString);
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
