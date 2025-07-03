
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;

namespace PetFamily.Application.Species.AddBreed
{
    public class AddBreedCommandHandler : ICommandHandler<Guid, AddBreedCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<AddBreedCommandHandler> _logger;

        public AddBreedCommandHandler(
            ISpeciesRepository speciesRepository,
            ILogger<AddBreedCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddBreedCommand command, 
            CancellationToken cancelationToken = default)
        {
            var speciesId = SpeciesId.Create(command.SpeciesId);

            var speciesResult = await _speciesRepository.GetById(speciesId, cancelationToken);

            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var species = speciesResult.Value;

            var breedId = BreedId.NewBreedId();
            var breedResult = Breed.Create(breedId, command.Title);

            if (breedResult.IsFailure)
                return breedResult.Error.ToErrorList();

            var breed = breedResult.Value;
            species.AddBreed(breed);

            await _speciesRepository.Save(species, cancelationToken);

            _logger.LogInformation("Created breed with title {title}", breed.Title);

            return breed.Id.Value;
        }
    }

}
