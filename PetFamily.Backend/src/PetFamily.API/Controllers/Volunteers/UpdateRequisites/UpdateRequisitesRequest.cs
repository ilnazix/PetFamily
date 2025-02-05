using PetFamily.Application.Volunteers.UpdateRequisites;

namespace PetFamily.API.Controllers.Volunteers.UpdateRequisites
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
