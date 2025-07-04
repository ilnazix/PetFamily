using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;

namespace PetFamily.Application.Species.Commands.Delete
{
    public class DeleteSpeciesCommandHandler : ICommandHandler<DeleteSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<DeleteSpeciesCommandHandler> _logger;

        public DeleteSpeciesCommandHandler(
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext,
            ILogger<DeleteSpeciesCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            DeleteSpeciesCommand command,
            CancellationToken cancelationToken = default)
        {
            var speciesId = SpeciesId.Create(command.Id);

            var speciesResult = await _speciesRepository.GetById(speciesId, cancelationToken);

            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var species = speciesResult.Value;

            var isExistPetWithSpecies = await _readDbContext.Pets
                .AnyAsync(p => p.SpeciesId == speciesId);

            if (isExistPetWithSpecies)
                return Errors.Species.CannotDeleteWhenAnimalsExist().ToErrorList();

            await _speciesRepository.Delete(species, cancelationToken);

            _logger.LogInformation("Species with title {1} deleted (id={2})", species.Title, species.Id);

            return UnitResult.Success<ErrorList>();
        }
    }
}
