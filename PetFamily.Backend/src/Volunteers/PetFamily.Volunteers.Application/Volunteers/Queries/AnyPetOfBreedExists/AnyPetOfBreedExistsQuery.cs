using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfBreedExists;

public record AnyPetOfBreedExistsQuery(Guid BreedId) : IQuery;