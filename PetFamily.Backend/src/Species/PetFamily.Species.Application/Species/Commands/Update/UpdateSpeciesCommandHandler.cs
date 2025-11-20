using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Application.Database;
using AnimalType = PetFamily.Species.Domain.Models.Species;


namespace PetFamily.Species.Application.Species.Commands.Update;

public class UpdateSpeciesCommandHandler : ICommandHandler<Guid, UpdateSpeciesCommand>
{
    private readonly ISpeciesUnitOfWork _unitOfWork;
    private readonly ISpeciesReadDbContext _readDbContext;
    private readonly ILogger<UpdateSpeciesCommandHandler> _logger;

    public UpdateSpeciesCommandHandler(
        ISpeciesUnitOfWork unitOfWork,
        ISpeciesReadDbContext readDbContext,
        ILogger<UpdateSpeciesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSpeciesCommand command,
        CancellationToken cancelationToken = default)
    {
        var isAlreadyExist = await _readDbContext.Species.AnyAsync(c => c.Title == command.Title, cancelationToken);
        if (isAlreadyExist)
            return Errors.General.ValueIsInvalid(nameof(AnimalType.Title)).ToErrorList();

        var speciesId = SpeciesId.Create(command.Id);
        var speciesResult = await _unitOfWork.SpeciesRepository.GetById(speciesId, cancelationToken);

        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();


        var result = speciesResult.Value.UpdateTitle(command.Title);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        _unitOfWork.Commit(cancelationToken);

        _logger.LogInformation("Title of species with {Id} has changed", speciesId.Value);

        return speciesId.Value;
    }
}
