using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class ConcertoUpCheck : ICheck
    {
        private const string Url = "http://concertodev.azurewebsites.net/";
        private const string Substring = "Events in Seattle";

        private readonly IHttpCheck _httpCheck;

        public ConcertoUpCheck(IHttpCheck httpCheck)
        {
            _httpCheck = httpCheck;
        }

        public string Name => "Concerto Up";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _httpCheck.ExecuteAsync(
                Url,
                Substring,
                token);
        }
    }
}
