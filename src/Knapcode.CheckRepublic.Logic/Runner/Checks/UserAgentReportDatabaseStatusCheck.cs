using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;
using Knapcode.CheckRepublic.Logic.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class UserAgentReportDatabaseStatusCheck : ICheck
    {
        private const string Url = "https://useragentreport.azurewebsites.net/api/v1/management/user-agent-database-status";
        private static readonly TimeSpan ErrorThreshold = TimeSpan.FromDays(1);

        private readonly IHttpJTokenCheck _check;
        private readonly ISystemClock _clock;

        public UserAgentReportDatabaseStatusCheck(ISystemClock clock, IHttpJTokenCheck check)
        {
            _check = check;
            _clock = clock;
        }

        public string Name => "User Agent Report Database Status";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            var now = _clock.UtcNow;

            return await _check.ExecuteAsync(
                Url,
                jToken =>
                {
                    var lastUploadedString = jToken.Value<string>("lastUpdated");
                    var lastUploaded = DateTimeOffset.Parse(lastUploadedString);
                    var sinceLastUploaded = now - lastUploaded;

                    if (sinceLastUploaded > ErrorThreshold)
                    {
                        return new CheckResultData
                        {
                            Type = CheckResultType.Failure,
                            Message = $"The last database update was at {lastUploaded} ({sinceLastUploaded} ago) which is greater than the error threshold, {ErrorThreshold}."
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
