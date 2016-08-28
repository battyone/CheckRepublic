using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class WintalloUpCheck : ICheck
    {
        private const string Url = "http://wintallo.com/";
        private const string Substring = "Thank you to all the fans who made this ride possible.";

        private readonly IHttpCheck _httpCheck;

        public WintalloUpCheck(IHttpCheck httpCheck)
        {
            _httpCheck = httpCheck;
        }

        public string Name => "Wintallo Up";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _httpCheck.ExecuteAsync(
                Url,
                Substring,
                token);
        }
    }
}
