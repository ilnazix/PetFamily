using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetPet
{
    public record GetPetQuery(Guid Id) : IQuery;
}
