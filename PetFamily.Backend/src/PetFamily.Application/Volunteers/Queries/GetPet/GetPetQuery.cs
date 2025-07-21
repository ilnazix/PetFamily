using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetPet
{
    public record GetPetQuery(Guid Id) : IQuery;
}
