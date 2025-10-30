using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;
using PetFamily.Species.Application.Species.Commands;

namespace PetFamily.Species.Application.Species.Commands.Delete;

public class DeleteSpeciesCommandHandler : ICommandHandler<DeleteSpeciesCommand>
{
    private readonly ISpeciesUnitOfWork _unitOfWork;
    private readonly IVolunteersModule _volunteersModule;
    private readonly ILogger<DeleteSpeciesCommandHandler> _logger;

    public DeleteSpeciesCommandHandler(
        ISpeciesUnitOfWork unitOfWork,
        IVolunteersModule volunteersModule,
        ILogger<DeleteSpeciesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _volunteersModule = volunteersModule;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeleteSpeciesCommand command,
        CancellationToken cancelationToken = default)
    {
        var speciesId = SpeciesId.Create(command.Id);

        var speciesResult = await _unitOfWork.SpeciesRepository.GetById(speciesId, cancelationToken);

        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var species = speciesResult.Value;

        var request = new AnyPetOfSpeciesExistsRequest(command.Id);
        var isExistPetWithSpecies = await _volunteersModule.AnyPetOfSpeciesExists(request, cancelationToken);
        if (isExistPetWithSpecies)
            return Errors.Species.CannotDeleteWhenAnimalsExist().ToErrorList();

        await _unitOfWork.SpeciesRepository.Delete(species, cancelationToken);
        await _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("Species with title {1} deleted (id={2})", species.Title, species.Id);

        return UnitResult.Success<ErrorList>();
    }
}