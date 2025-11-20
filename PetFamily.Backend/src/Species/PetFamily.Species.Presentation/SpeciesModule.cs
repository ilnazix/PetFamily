using PetFamily.Species.Application.Species.Queries.CheckIfBreedsExistsQuery;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Species.Presentation.Extensions;

namespace PetFamily.Species.Presentation;

internal class SpeciesModule : ISpeciesModule
{
    private readonly CheckBreedsExistenceQueryHandler _checkBreedsExistenceQueryHandler;

    public SpeciesModule(
        CheckBreedsExistenceQueryHandler checkBreedsExistenceQueryHandler)
    {
        _checkBreedsExistenceQueryHandler = checkBreedsExistenceQueryHandler;
    }

    public async Task<bool> CheckBreedsExistence(
        CheckBreedExistenceRequest request,
        CancellationToken cancellationToken)
    {
        return await _checkBreedsExistenceQueryHandler
            .Handle(request.ToQuery() ,cancellationToken);
    }
}
