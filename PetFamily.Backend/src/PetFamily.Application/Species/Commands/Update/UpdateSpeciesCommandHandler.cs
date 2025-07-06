using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using AnimalType = PetFamily.Domain.Species.Species;


namespace PetFamily.Application.Species.Commands.Update
{
    public class UpdateSpeciesCommandHandler : ICommandHandler<Guid, UpdateSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<UpdateSpeciesCommandHandler> _logger;

        public UpdateSpeciesCommandHandler(
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext,
            ILogger<UpdateSpeciesCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
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
            var speciesResult = await _speciesRepository.GetById(speciesId, cancelationToken);

            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();


            var result = speciesResult.Value.UpdateTitle(command.Title);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _speciesRepository.Save(speciesResult.Value, cancelationToken);

            _logger.LogInformation("Title of species with {Id} has changed", speciesId.Value);

            return speciesId.Value;
        }
    }
}
