using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Species
{
    public interface ISpeciesRepository
    {
        Task<Result<Domain.Species.Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default);
        Task<Guid> Add(Domain.Species.Species species, CancellationToken cancellationToken = default);
        Task<bool> IsAlreadyExistWithTitle(string title, CancellationToken cancellationToken = default);
        Task<Result<Guid, Error>> Save(Domain.Species.Species species, CancellationToken cancellationToken = default);
    }
}
