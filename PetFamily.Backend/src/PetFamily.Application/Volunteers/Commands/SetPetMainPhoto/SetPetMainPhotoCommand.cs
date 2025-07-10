using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.SetPetMainPhoto
{
    public record SetPetMainPhotoCommand(
        Guid VolunteerId, 
        Guid PetId, 
        string ImagePath) : ICommand;
}
