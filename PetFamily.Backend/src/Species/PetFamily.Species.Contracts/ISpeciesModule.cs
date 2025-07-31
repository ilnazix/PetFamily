using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Contracts
{
    public interface ISpeciesModule
    {
        Task<bool> CheckBreedsExistence(CheckBreedExistenceRequest request, CancellationToken cancellationToken);
    }
}
