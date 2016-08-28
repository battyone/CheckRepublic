using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class BlogUpCheck : ICheck
    {
        private const string Url = "http://joelverhagen.com/";
        private const string Substring = "a computer programming blog";

        private readonly IHttpCheck _httpCheck;

        public BlogUpCheck(IHttpCheck httpCheck)
        {
            _httpCheck = httpCheck;
        }

        public string Name => "Blog Up";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _httpCheck.ExecuteAsync(
                Url,
                Substring,
                token);
        }
    }
}
