using PetFamily.API.Controllers.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateRequisites
{
    public record UpdateRequisitesCommand(Guid Id, UpdateRequisitesDto Dto);

    public record UpdateRequisitesDto(RequisitesDto[] Requisites);
}
