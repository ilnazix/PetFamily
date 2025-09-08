using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using System.IdentityModel.Tokens.Jwt;

namespace PetFamily.Framework.Auth;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var userIdClaim = context.User
            .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if(!Guid.TryParse(userIdClaim, out var userId))
        {
            context.Fail();
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var accountsContract = scope.ServiceProvider
            .GetRequiredService<IAccountsModule>();

        var userPermissionCodes = await accountsContract.GetUserPermissionCodes(userId);

        if (userPermissionCodes.Contains(requirement.Code))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
