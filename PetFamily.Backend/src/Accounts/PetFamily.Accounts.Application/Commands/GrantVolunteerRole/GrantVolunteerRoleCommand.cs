using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.GrantVolunteerRole;

public record GrantVolunteerRoleCommand(Guid UserId) : ICommand;