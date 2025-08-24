using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command,
        CancellationToken cancelationToken = default)
    {
        var user = new User
        {
            Email = command.Email,
            UserName = command.UserName
        };

        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Created user with email {0}", command.Email);
            return UnitResult.Success<ErrorList>();
        }

        var errors = result.Errors
            .Select(e => Error.Failure(e.Code, e.Description))
            .ToList();

        return new ErrorList(errors);
    }
}
