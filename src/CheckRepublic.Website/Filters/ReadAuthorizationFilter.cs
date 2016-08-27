using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Knapcode.CheckRepublic.Website.Filters
{
    public class ReadAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IOptions<WebsiteOptions> _options;

        public ReadAuthorizationFilter(IOptions<WebsiteOptions> options)
        {
            _options = options;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            BearerTokenUtility.ValidateBearerToken(_options.Value.ReadToken, context);
        }
    }
}
