using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Knapcode.CheckRepublic.Website.Authorization
{
    public class AnonymousHandler : AuthorizationHandler<AnonymousRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnonymousRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
