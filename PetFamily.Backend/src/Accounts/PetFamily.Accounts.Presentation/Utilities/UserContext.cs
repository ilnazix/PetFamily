using Microsoft.AspNetCore.Http;
using PetFamily.Framework.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PetFamily.Accounts.Presentation.Utilities;

internal class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserScopedData Current
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;

            if (user?.Identity is not { IsAuthenticated: true })
                return new UserScopedData();

            return new UserScopedData
            {
                UserId = Guid.TryParse(user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out var id) ? id : Guid.Empty,
                Email = user.FindFirst(JwtRegisteredClaimNames.Email)?.Value ?? string.Empty
            };
        }
    }
}
