using PetFamily.Application.Volunteers.Commands.ChangePetPosition;
using System.Security.Cryptography.X509Certificates;

namespace PetFamily.API.Controllers.Volunteers.Requests
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
