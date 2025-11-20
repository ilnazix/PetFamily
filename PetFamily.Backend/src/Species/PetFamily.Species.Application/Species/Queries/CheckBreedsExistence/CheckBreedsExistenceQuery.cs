using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Species.Queries.CheckIfBreedsExistsQuery;

public record CheckBreedsExistenceQuery(Guid SpeciesId, Guid BreedId) : IQuery;
