using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.DeletePermanently
{
    public record DeleteVolunteerPermanentlyCommand(Guid Id) : ICommand;
}
