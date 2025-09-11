using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Commands;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Accounts.Infrastructure.Options.Jwt;
using PetFamily.Accounts.Infrastructure.Options.RefreshSession;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetFamily.Accounts.Infrastructure.Providers;

internal class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly RefreshSessionOptions _refreshSessionOptions;
    private readonly RefreshSessionManager _refreshSessionManager;

    public JwtTokenProvider(
        IOptions<JwtOptions> jwtOptions,
        IOptions<RefreshSessionOptions> refreshSessionOptions,
        RefreshSessionManager refreshSessionManager)
    {
        _jwtOptions = jwtOptions.Value;
        _refreshSessionOptions = refreshSessionOptions.Value;
        _refreshSessionManager = refreshSessionManager;
    }

    public string GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = ConfigureClaims(user);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeInMinutes),
            signingCredentials: signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;
    }

    public async Task<string> GenerateRefreshTokenAsync(
        User user, 
        LoginMetadata metadata,
        CancellationToken cancellationToken)
    {
        var refreshToken = Guid.NewGuid();

        var refreshSession = new RefreshSession 
        { 
            User = user,
            Fingerprint = metadata.Fingerprint,
            UserAgent = metadata.UserAgent,
            IP = metadata.IP,
            CreatedAt = DateTime.UtcNow,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshSessionOptions.RefreshTokenLifetimeInDays),
        };

        await _refreshSessionManager.Save(refreshSession, cancellationToken);

        return refreshToken.ToString();
    }

    private Claim[] ConfigureClaims(User user)
    {
        List<Claim> claims = 
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
        ];

        var roleClaims = user.Roles.Select(r => new Claim("Role", r.Name!));

        claims.AddRange(roleClaims);

        return claims.ToArray();
    }
}
