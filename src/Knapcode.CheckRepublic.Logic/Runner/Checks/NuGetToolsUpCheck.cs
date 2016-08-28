using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class NuGetToolsUpCheck : ICheck
    {
        private const string Url = "http://nugettoolsdev.azurewebsites.net/3.5.0-rc1-final/parse-framework?framework=.netframework%2Cversion%3Dv4.0";
        private const string Substring = ".NETFramework,Version=v4.0";
        
        private readonly IHttpSubstringCheck _httpCheck;

        public NuGetToolsUpCheck(IHttpSubstringCheck httpCheck)
        {
            _httpCheck = httpCheck;
        }

        public string Name => "NuGet Tools Up";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _httpCheck.ExecuteAsync(
                Url,
                Substring,
                token);
        }
    }
}
