using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Species.Application.Species.Commands.DeleteBreed
{
    public class DeleteBreedCommandHandler : ICommandHandler<DeleteBreedCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<DeleteBreedCommandHandler> _logger;

        public DeleteBreedCommandHandler(
            ISpeciesRepository speciesRepository,
            ILogger<DeleteBreedCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
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

            //TODO: Сделать проверку что нет питомцев такого вида
            /*var isExistPetWithBreed = await _readDbContext.Pets.AnyAsync(p => p.BreedId == breedId.Value, cancelationToken);
            if (isExistPetWithBreed)
                return Errors.Breeds.CannotDeleteWhenAnimalsExist().ToErrorList();*/

            var result = species.DeleteBreedById(breedId);
            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _speciesRepository.Save(species, cancelationToken);

            _logger.LogInformation("Breeds deleted (id={2})", breedId.Value);

            return UnitResult.Success<ErrorList>();
        }
    }
}
