using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Commands.RefreshToken;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Login;

public class LoginUserCommandHandler : ICommandHandler<LoginUserResponse, LoginUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        IRefreshSessionManager refreshSessionManager,
        ILogger<LoginUserCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _refreshSessionManager = refreshSessionManager;
        _logger = logger;
    }

    public async Task<Result<LoginUserResponse, ErrorList>> Handle(
        LoginUserCommand command,
        CancellationToken cancelationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null) return Errors.User.InvalidCredentials().ToErrorList();

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);

        if (!passwordConfirmed) return Errors.User.InvalidCredentials().ToErrorList();

        var accessToken = _tokenProvider.GenerateAccessToken(user);

        var metadata = command.Metadata;
        var refreshSession = _tokenProvider.GenerateRefreshToken(
            user, 
            metadata);

        var result = new LoginUserResponse(accessToken, refreshSession.RefreshToken.ToString());

        await _refreshSessionManager.Add(refreshSession, cancelationToken);
        await _refreshSessionManager.Save(cancelationToken);

        _logger.LogInformation("User with email {0} successfully loged in", user.Email);

        return result;
    }
}
