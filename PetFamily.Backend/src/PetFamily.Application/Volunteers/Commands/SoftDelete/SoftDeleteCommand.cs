using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.SoftDelete
{
    public record SoftDeleteCommand(Guid Id) : ICommand;
}
