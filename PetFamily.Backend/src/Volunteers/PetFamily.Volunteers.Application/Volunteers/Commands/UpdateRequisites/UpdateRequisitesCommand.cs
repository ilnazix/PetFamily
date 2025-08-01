using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites
{
    public record UpdateRequisitesCommand(Guid Id, IEnumerable<RequisiteInfo> Requisites) : ICommand;
}
