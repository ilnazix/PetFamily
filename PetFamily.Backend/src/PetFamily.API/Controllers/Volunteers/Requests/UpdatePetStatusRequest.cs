using PetFamily.Application.Volunteers.Commands.UpdatePetStatus;

namespace PetFamily.API.Controllers.Volunteers.Requests
{
    public record UpdatePetStatusRequest(string Status) 
    {
        public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
            => new(volunteerId, petId, Status);
    }

}
