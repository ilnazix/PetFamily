using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<LoginUserResponse, RefreshTokenCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly UserManager<User> _userManager;

    public RefreshTokenCommandHandler(
        IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        UserManager<User> userManager)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _userManager = userManager;
    }

    public async Task<Result<LoginUserResponse, ErrorList>> Handle(
        RefreshTokenCommand command, 
        CancellationToken cancelationToken = default)
    {
        var sessionResult = await _refreshSessionManager
            .GetByRefreshToken(command.RefreshToken, cancelationToken);

        if (sessionResult.IsFailure)
            return sessionResult.Error.ToErrorList();

        var oldRefreshSession = sessionResult.Value;

        if (!IsSessionValid(oldRefreshSession, command))
            return Errors.User.TokenExpired().ToErrorList();

        var user = oldRefreshSession.User;

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshSession = _tokenProvider
            .GenerateRefreshToken(user, command.Metadata);

        string refreshToken = refreshSession.RefreshToken.ToString();
        var result = new LoginUserResponse(accessToken, refreshToken);

        _refreshSessionManager.Delete(oldRefreshSession);
        await _refreshSessionManager.Add(refreshSession, cancelationToken);
        await _refreshSessionManager.Save(cancelationToken);

        return result;
    }

    private static bool IsSessionValid(RefreshSession session, 
        RefreshTokenCommand command)
    {
        return session.ExpiresAt > DateTime.UtcNow
                    && session.Fingerprint == command.Metadata.Fingerprint;
    }
}
