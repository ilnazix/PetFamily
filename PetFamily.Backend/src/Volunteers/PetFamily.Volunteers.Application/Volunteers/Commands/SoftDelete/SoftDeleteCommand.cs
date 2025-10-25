using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.SoftDelete;

public record SoftDeleteCommand(Guid Id) : ICommand;
