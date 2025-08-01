using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Contracts
{
    public interface IVolunteersModule
    {
        Task<bool> AnyPetOfBreedExists(AnyPetOfBreedExistsRequest request, CancellationToken cancellationToken);
        Task<bool> AnyPetOfSpeciesExists(AnyPetOfSpeciesExistsRequest request, CancellationToken cancellationToken);
    }
}
