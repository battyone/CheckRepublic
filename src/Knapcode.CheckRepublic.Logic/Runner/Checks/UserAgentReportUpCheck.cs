using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class UserAgentReportUpCheck : ICheck
    {
        private const string Url = "https://useragentreport.azurewebsites.net/api/v1/top-user-agents?limit=5";
        private const int ExpectedCount = 5;

        private readonly IHttpJTokenCheck _check;

        public UserAgentReportUpCheck(IHttpJTokenCheck check)
        {
            _check = check;
        }

        public string Name => "User Agent Report Up";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _check.ExecuteAsync(
                Url,
                jToken =>
                {
                    var actualCount = jToken.Count();
                    if (actualCount != ExpectedCount)
                    {
                        return new CheckResultData
                        {
                            Type = CheckResultType.Failure,
                            Message = $"There are {actualCount} (not {ExpectedCount}) entries in the top-user-agents endpoint."
                        };
                    }

                    if (jToken.Any(x => x.Value<string>("userAgent") == null))
                    {
                        return new CheckResultData
                        {
                            Type = CheckResultType.Failure,
                            Message = $"One of the entries on the top-user-agents endpoint has not user agent."
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
