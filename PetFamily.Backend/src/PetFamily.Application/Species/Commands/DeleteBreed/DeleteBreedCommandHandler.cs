using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;

namespace PetFamily.Application.Species.Commands.DeleteBreed
{
    public class DeleteBreedCommandHandler : ICommandHandler<DeleteBreedCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<DeleteBreedCommandHandler> _logger;

        public DeleteBreedCommandHandler(
            ISpeciesRepository speciesRepository,
            IReadDbContext readDbContext,
            ILogger<DeleteBreedCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _readDbContext = readDbContext;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            DeleteBreedCommand command, 
            CancellationToken cancelationToken = default)
        {
            var speciesId = SpeciesId.Create(command.SpeciesId);
            var speciesResult = await _speciesRepository.GetById(speciesId, cancelationToken);
            
            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var species = speciesResult.Value;
            var breedId = BreedId.Create(command.BreedId);

            var isExistPetWithBreed = await _readDbContext.Pets.AnyAsync(p => p.BreedId == breedId.Value, cancelationToken);
            if (isExistPetWithBreed)
                return Errors.Breeds.CannotDeleteWhenAnimalsExist().ToErrorList();

            var result = species.DeleteBreedById(breedId);
            if(result.IsFailure)
                return result.Error.ToErrorList();

            await _speciesRepository.Save(species, cancelationToken);

            _logger.LogInformation("Breeds deleted (id={2})", breedId.Value);

            return UnitResult.Success<ErrorList>();
        }
    }
}
