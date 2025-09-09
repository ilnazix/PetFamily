using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Framework.Auth;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
