using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;

namespace PetFamily.Application.Species
{
    public interface ISpeciesRepository
    {
        Task<Result<Domain.Species.Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default);
    }
}
