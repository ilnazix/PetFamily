namespace PetFamily.Species.Contracts.Requests;

public record CheckBreedExistenceRequest(Guid SpeciesId, Guid BreedId);
