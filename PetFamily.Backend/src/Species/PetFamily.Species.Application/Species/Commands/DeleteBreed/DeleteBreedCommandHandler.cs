using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Species.Application.Species.Commands.DeleteBreed;

public class DeleteBreedCommandHandler : ICommandHandler<DeleteBreedCommand>
{
    private readonly ISpeciesUnitOfWork _unitOfWork;
    private readonly IVolunteersModule _volunteersModule;
    private readonly ILogger<DeleteBreedCommandHandler> _logger;

    public DeleteBreedCommandHandler(
        ISpeciesUnitOfWork unitOfWork,
        IVolunteersModule volunteersModule,
        ILogger<DeleteBreedCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _volunteersModule = volunteersModule;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeleteBreedCommand command,
        CancellationToken cancelationToken = default)
    {
        var speciesId = SpeciesId.Create(command.SpeciesId);
        var speciesResult = await _unitOfWork.SpeciesRepository.GetById(speciesId, cancelationToken);

        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var species = speciesResult.Value;
        var breedId = BreedId.Create(command.BreedId);

        
        var isExistPetWithBreed = await _volunteersModule
            .AnyPetOfBreedExists(new AnyPetOfBreedExistsRequest(command.BreedId), cancelationToken);
        if (isExistPetWithBreed)
            return Errors.Breeds.CannotDeleteWhenAnimalsExist().ToErrorList();

        var result = species.DeleteBreedById(breedId);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("Breeds deleted (id={2})", breedId.Value);

        return UnitResult.Success<ErrorList>();
    }
}
