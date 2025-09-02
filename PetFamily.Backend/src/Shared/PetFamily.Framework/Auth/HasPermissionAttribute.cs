using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Framework.Auth;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {   
    }
}
