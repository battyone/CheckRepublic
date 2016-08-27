using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Knapcode.CheckRepublic.Website.Filters
{
    public static class BearerTokenUtility
    {
        public static void ValidateBearerToken(string expectedToken, AuthorizationFilterContext context)
        {
            if (!IsAuthorized(expectedToken, context))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private static bool IsAuthorized(string expectedToken, AuthorizationFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(expectedToken))
            {
                return true;
            }

            var authorizationHeader = context
                .HttpContext
                .Request
                .Headers["Authorization"]
                .FirstOrDefault() ?? string.Empty;

            var pieces = authorizationHeader.Split(new[] { ' ' }, 2);

            if (pieces.Length < 2 || pieces[0] != "bearer")
            {
                return false;
            }

            var actualToken = pieces[1];

            return expectedToken == actualToken;
        }
    }
}
