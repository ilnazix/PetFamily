using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Application.Species.Commands.AddBreed;

namespace PetFamily.Species.Application.Species.Commands.UpdateBreed
{
    public class UpdateBreedCommandHandler : ICommandHandler<Guid, UpdateBreedCommand>
    {
        private readonly ISpeciesUnitOfWork _unitOfWork;
        private readonly ILogger<AddBreedCommandHandler> _logger;

        public UpdateBreedCommandHandler(
            ISpeciesUnitOfWork unitOfWork,
            ILogger<AddBreedCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(UpdateBreedCommand command, CancellationToken cancelationToken = default)
        {
            var speciesId = SpeciesId.Create(command.SpeciesId);
            var speciesResult = await _unitOfWork.SpeciesRepository.GetById(speciesId, cancelationToken);

            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var species = speciesResult.Value;
            var breedId = BreedId.Create(command.BreedId);

            var result = species.UpdateBreedTitle(breedId, command.Title);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _unitOfWork.Commit(cancelationToken);

            _logger.LogInformation("Updated breed title {title}", command.Title);

            return breedId.Value;
        }
    }
}
