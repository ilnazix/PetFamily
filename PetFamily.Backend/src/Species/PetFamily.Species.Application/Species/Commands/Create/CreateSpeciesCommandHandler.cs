using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Application.Database;
using AnimalType = PetFamily.Species.Domain.Models.Species;


namespace PetFamily.Species.Application.Species.Commands.Create
{
    public class CreateSpeciesCommandHandler : ICommandHandler<Guid, CreateSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ISpeciesReadDbContext _readDbContext;
        private readonly ILogger<CreateSpeciesCommandHandler> _logger;

        public CreateSpeciesCommandHandler(
            ISpeciesRepository speciesRepository,
            ISpeciesReadDbContext readDbContext,
            ILogger<CreateSpeciesCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            CreateSpeciesCommand command,
            CancellationToken cancelationToken = default)
        {
            var isSpeciesAlreadyExist = await _readDbContext.Species.AnyAsync(c => c.Title == command.Title, cancelationToken);

            if (isSpeciesAlreadyExist)
                return Errors.General.ValueIsInvalid(nameof(AnimalType.Title)).ToErrorList();

            var speeciesId = SpeciesId.NewSpeciesId();
            var speciesResult = AnimalType.Create(speeciesId, command.Title);

            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var id = await _speciesRepository.Add(speciesResult.Value, cancelationToken);

            _logger.LogInformation("Created species with {title}", command.Title);

            return id;
        }
    }
}
