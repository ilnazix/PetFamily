using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Domain.Models;

namespace PetFamily.Species.Application.Species.Commands.AddBreed;

public class AddBreedCommandHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly ISpeciesUnitOfWork _unitOfWork;
    private readonly ILogger<AddBreedCommandHandler> _logger;

    public AddBreedCommandHandler(
        ISpeciesUnitOfWork unitOfWork,
        ILogger<AddBreedCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedCommand command,
        CancellationToken cancelationToken = default)
    {
        var speciesId = SpeciesId.Create(command.SpeciesId);

        var speciesResult = await _unitOfWork.SpeciesRepository.GetById(speciesId, cancelationToken);

        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var species = speciesResult.Value;

        var breedId = BreedId.NewBreedId();
        var breedResult = Breed.Create(breedId, command.Title);

        if (breedResult.IsFailure)
            return breedResult.Error.ToErrorList();

        var breed = breedResult.Value;
        species.AddBreed(breed);

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("Created breed with title {title}", breed.Title);

        return breed.Id.Value;
    }
}
