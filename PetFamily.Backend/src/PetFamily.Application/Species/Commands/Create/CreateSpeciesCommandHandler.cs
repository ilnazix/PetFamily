using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using AnimalType = PetFamily.Domain.Species.Species;


namespace PetFamily.Application.Species.Commands.Create
{
    public class CreateSpeciesCommandHandler : ICommandHandler<Guid, CreateSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<CreateSpeciesCommandHandler> _logger;

        public CreateSpeciesCommandHandler(
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext,
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
