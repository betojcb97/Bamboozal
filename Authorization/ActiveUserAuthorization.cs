using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bamboo.Authorization
{
    public class ActiveUserAuthorization : AuthorizationHandler<ActiveUser>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUser requirement)
        {
            var isActive = context.User.FindFirst(claim => claim.Type == "isActive");
            if (isActive.Value == "false" || isActive.Value is null) return Task.CompletedTask;
            else { context.Succeed(requirement); return Task.CompletedTask; }
        }
    }
}
