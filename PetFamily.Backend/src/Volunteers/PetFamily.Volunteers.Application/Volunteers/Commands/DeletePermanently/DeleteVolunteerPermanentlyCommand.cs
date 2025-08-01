using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePermanently
{
    public record DeleteVolunteerPermanentlyCommand(Guid Id) : ICommand;
}
