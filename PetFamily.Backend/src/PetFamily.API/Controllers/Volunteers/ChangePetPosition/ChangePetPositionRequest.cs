using PetFamily.Application.Volunteers.ChangePetPosition;
using System.Security.Cryptography.X509Certificates;

namespace PetFamily.API.Controllers.Volunteers.ChangePetPosition
{
    public record ChangePetPositionRequest
    {
        public int NewPosition { get; init; }
        public ChangePetPositionCommand ToCommand(Guid volunteerId, Guid petId)
        {
            var command = new ChangePetPositionCommand(volunteerId, petId, NewPosition);
            return command;
        }
    }
}
