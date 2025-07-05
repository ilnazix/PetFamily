using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;

namespace PetFamily.Application.Species.Commands
{
    public interface ISpeciesRepository
    {
        Task<Result<Domain.Species.Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default);
        Task<Guid> Add(Domain.Species.Species species, CancellationToken cancellationToken = default);
        Task<Result<Guid, Error>> Save(Domain.Species.Species species, CancellationToken cancellationToken = default);
        Task<Result<Guid, Error>> Delete(Domain.Species.Species species, CancellationToken cancelationToken);
    }
}
