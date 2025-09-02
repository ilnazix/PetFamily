using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Framework.Auth;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var permissionClaims = context.User.FindAll(x => x.Type == "Permission");

        if (permissionClaims.Any(c => c.Value == requirement.Code)) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
