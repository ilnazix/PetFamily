using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.SetPetMainPhoto;

public record SetPetMainPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string ImagePath) : ICommand;
