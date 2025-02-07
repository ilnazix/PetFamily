using PetFamily.API.Controllers.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateRequisites
{
    public record UpdateRequisitesCommand(Guid Id, IEnumerable<RequisitesInfo> Requisites);
}
