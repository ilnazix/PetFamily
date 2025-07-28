using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.DeletePet
{
    public class DeletePetCommandHandler : ICommandHandler<DeletePetCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<DeletePetCommandHandler> _logger;

        public DeletePetCommandHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<DeletePetCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorList>> Handle(
            DeletePetCommand command,
            CancellationToken cancelationToken = default)
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancelationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;
            var petId = PetId.Create(command.PetId);
            var deletePetResult = volunteer.DeletePet(petId);

            if (deletePetResult.IsFailure)
                return deletePetResult.Error.ToErrorList();

            await _volunteersRepository.Save(volunteer, cancelationToken);

            _logger.LogInformation(
                "Pet with ID={petID} deleted by volunteer with ID={volunteerID}",
                petId.Value,
                volunteerId.Value
                );

            return UnitResult.Success<ErrorList>();
        }
    }
}
