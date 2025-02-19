using PetFamily.Application.Volunteers.Commands.Shared;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;

namespace PetFamily.API.Controllers.Volunteers.Requests
{
    public record UpdateRequisitesRequest
    {
        public IEnumerable<RequisiteDto> Requisites { get; }

        public UpdateRequisitesRequest(IEnumerable<RequisiteDto> requisites)
        {
            Requisites = requisites;
        }

        public UpdateRequisitesCommand ToCommand(Guid id)
        {
            var requisites = Requisites.Select(r => new RequisitesInfo(r.Title, r.Description));
            var command = new UpdateRequisitesCommand(id, requisites);

            return command;
        }
    }
}
