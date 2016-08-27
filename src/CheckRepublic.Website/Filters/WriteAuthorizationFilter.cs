using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Knapcode.CheckRepublic.Website.Filters
{
    public class WriteAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IOptions<WebsiteOptions> _options;

        public WriteAuthorizationFilter(IOptions<WebsiteOptions> options)
        {
            _options = options;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            BearerTokenUtility.ValidateBearerToken(_options.Value.WriteToken, context);
        }
    }
}
