using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Login;

public class LoginUserCommandHandler : ICommandHandler<LoginUserResponse, LoginUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginUserCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
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

        var accessToken = _tokenProvider.GenerateAccessToken(user, cancelationToken);

        var metadata = command.Metadata;
        var refreshToken = await _tokenProvider.GenerateRefreshTokenAsync(
            user, 
            metadata, 
            cancelationToken);

        var result = new LoginUserResponse(accessToken, refreshToken);

        _logger.LogInformation("User with email {0} successfully loged in", user.Email);

        return result;
    }
}
