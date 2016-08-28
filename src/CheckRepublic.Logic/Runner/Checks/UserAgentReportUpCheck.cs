using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class UserAgentReportUpCheck : ICheck
    {
        private const string Url = "http://useragentreport.azurewebsites.net/api/v1/top-user-agents";
        private const string Substring = "\"UserAgent\":";

        private readonly IHttpCheck _httpCheck;

        public UserAgentReportUpCheck(IHttpCheck httpCheck)
        {
            _httpCheck = httpCheck;
        }

        public string Name => "User Agent Report Up";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _httpCheck.ExecuteAsync(
                Url,
                Substring,
                token);
        }
    }
}
