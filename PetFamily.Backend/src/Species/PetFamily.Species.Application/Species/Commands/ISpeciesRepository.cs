using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Species.Application.Species.Commands
{
    public interface ISpeciesRepository
    {
        Task<Result<Domain.Models.Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default);
        Task<Guid> Add(Domain.Models.Species species, CancellationToken cancellationToken = default);
        Task<Result<Guid, Error>> Save(Domain.Models.Species species, CancellationToken cancellationToken = default);
        Task<Result<Guid, Error>> Delete(Domain.Models.Species species, CancellationToken cancelationToken);
    }
}
