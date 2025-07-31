using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Species.Application.Species.Commands.Delete
{
    public class DeleteSpeciesCommandHandler : ICommandHandler<DeleteSpeciesCommand>
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IVolunteersModule _volunteersModule;
        private readonly ILogger<DeleteSpeciesCommandHandler> _logger;

        public DeleteSpeciesCommandHandler(
            ISpeciesRepository speciesRepository,
            IVolunteersModule volunteersModule,
            ILogger<DeleteSpeciesCommandHandler> logger)
        {
            _speciesRepository = speciesRepository;
            _volunteersModule = volunteersModule;
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

            var request = new AnyPetOfSpeciesExistsRequest(command.Id);
            var isExistPetWithSpecies = await _volunteersModule.AnyPetOfSpeciesExists(request, cancelationToken);
            if (isExistPetWithSpecies)
                return Errors.Species.CannotDeleteWhenAnimalsExist().ToErrorList();

            await _speciesRepository.Delete(species, cancelationToken);

            _logger.LogInformation("Species with title {1} deleted (id={2})", species.Title, species.Id);

            return UnitResult.Success<ErrorList>();
        }
    }
}