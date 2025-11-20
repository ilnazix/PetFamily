using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.GrantVolunteerRole;

public class GrantVolunteerRoleCommandHandler : ICommandHandler<GrantVolunteerRoleCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<GrantVolunteerRoleCommandHandler> _logger;

    public GrantVolunteerRoleCommandHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<GrantVolunteerRoleCommandHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(GrantVolunteerRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());
        if (user == null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        var roleExists = await _roleManager.RoleExistsAsync(VolunteerAccount.ROLE);
        if (!roleExists)
            return Errors.General.NotFound().ToErrorList();

        var result = await _userManager.AddToRoleAsync(user, VolunteerAccount.ROLE);
        if (result.Succeeded)
        {
            _logger.LogInformation(
                "User {UserId} successfully granted the '{Role}' role.", 
                user.Id, 
                VolunteerAccount.ROLE);
            return UnitResult.Success<ErrorList>();
        }

        var errors = result.Errors
            .Select(e => Error.Failure(e.Code, e.Description))
            .ToList();

        _logger.LogWarning("Failed to grant role '{Role}' to user {UserId}: {Errors}",
            VolunteerAccount.ROLE, user.Id,
            string.Join(", ", errors.Select(e => $"{e.Code}: {e.Message}")));

        return new ErrorList(errors);
    }
}
