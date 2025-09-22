using Microsoft.AspNetCore.Http;

namespace PetFamily.Accounts.Presentation.Extensions;

internal static class CookiesExtensions
{
    public static void SetHttpOnlyCookie(
        this IResponseCookies cookies,
        string cookieName,
        string value,
        int expiresIn)
    {
        var options = new CookieOptions() 
        { 
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(expiresIn),
        };


        cookies.Append(cookieName, value, options);
    }
}
