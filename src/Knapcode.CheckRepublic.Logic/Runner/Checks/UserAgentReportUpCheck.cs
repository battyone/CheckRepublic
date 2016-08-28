using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class UserAgentReportUpCheck : ICheck
    {
        private const string Url = "http://useragentreport.azurewebsites.net/api/v1/top-user-agents";
        private const string Substring = "\"UserAgent\":";

        private readonly IHttpSubstringCheck _httpCheck;

        public UserAgentReportUpCheck(IHttpSubstringCheck httpCheck)
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
