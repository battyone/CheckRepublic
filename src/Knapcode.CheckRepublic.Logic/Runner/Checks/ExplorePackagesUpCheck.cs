using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;

namespace Knapcode.CheckRepublic.Logic.Runner.Checks
{
    public class ExplorePackagesUpCheck : ICheck
    {
        private const string Url = "http://explorepackages.azurewebsites.net/";
        private const string Substring = "Enter a package ID and version below to check its consistency on the various NuGet API endpoints on NuGet.org.";
        
        private readonly IHttpSubstringCheck _httpCheck;

        public ExplorePackagesUpCheck(IHttpSubstringCheck httpCheck)
        {
            _httpCheck = httpCheck;
        }

        public string Name => "Explore Packages Up";

        public async Task<CheckResultData> ExecuteAsync(CancellationToken token)
        {
            return await _httpCheck.ExecuteAsync(
                Url,
                Substring,
                token);
        }
    }
}
