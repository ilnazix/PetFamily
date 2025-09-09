using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command,
        CancellationToken cancelationToken = default)
    {
        var role = await _roleManager.FindByNameAsync(ParticipantAccount.ROLE);

        var userResult = User.CreateParticipant(command.Email, command.UserName, role!);

        if (userResult.IsFailure) return userResult.Error.ToErrorList();

        var user = userResult.Value;

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
