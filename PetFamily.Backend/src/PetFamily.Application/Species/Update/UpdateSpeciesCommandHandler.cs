using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using AnimalType = PetFamily.Domain.Species.Species;


namespace PetFamily.Application.Species.Update
{
    public class UpdateSpeciesCommandHandler : ICommandHandler<Guid, UpdateSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly ILogger<UpdateSpeciesCommandHandler> _logger;

        public UpdateSpeciesCommandHandler(
            ISpeciesRepository speciesRepository,
            ILogger<UpdateSpeciesCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateSpeciesCommand command, 
            CancellationToken cancelationToken = default)
        {
            var isAlreadyExist = await _speciesRepository.IsAlreadyExistWithTitle(command.Title, cancelationToken);
            if (isAlreadyExist)
                return Errors.General.ValueIsInvalid(nameof(AnimalType.Title)).ToErrorList();

            var speciesId = SpeciesId.Create(command.Id);
            var speciesResult = await _speciesRepository.GetById(speciesId, cancelationToken);

            if(speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();


            var result = speciesResult.Value.UpdateTitle(command.Title);

            if(result.IsFailure)
                return result.Error.ToErrorList();

            await _speciesRepository.Save(speciesResult.Value, cancelationToken);

            _logger.LogInformation("Title of species with {id} has changed", speciesId.Value);

            return speciesId.Value;
        }
    }
}
