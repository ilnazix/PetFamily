using PetFamily.Application.Volunteers.Commands.SetPetMainPhoto;

namespace PetFamily.API.Controllers.Volunteers.Requests
{
    public record SetPetMainPhotoRequest
    {
        public string ImagePath { get; init; } = string.Empty;

        public SetPetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId) 
            => new(volunteerId, petId, ImagePath);
        
    }

}
