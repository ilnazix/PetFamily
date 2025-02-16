using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.HardDelete
{
    public record HardDeleteCommand(Guid Id) : ICommand;
}
