using Microsoft.AspNetCore.Http;
using PetFamily.Accounts.Application.Commands;

namespace PetFamily.Accounts.Presentation.Extensions;
internal static class HttpRequestExtensions
{
    public static LoginMetadata GetLoginMetadata(
        this HttpRequest request,
        string fingerPrint)
    {
        var userAgent = request.Headers.UserAgent.ToString();
        var ip = request.Headers.ContainsKey("X-Forwarded-For")
            ? request.Headers["X-Forwarded-For"].ToString().Split(',').First().Trim()
            : request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var metadata = new LoginMetadata(userAgent, ip, fingerPrint);

        return metadata;
    }
}
