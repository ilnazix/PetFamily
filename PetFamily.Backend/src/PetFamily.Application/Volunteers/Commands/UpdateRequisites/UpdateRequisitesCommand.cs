using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.Shared;

namespace PetFamily.Application.Volunteers.Commands.UpdateRequisites
{
    public record UpdateRequisitesCommand(Guid Id, IEnumerable<RequisitesInfo> Requisites) : ICommand;
}
