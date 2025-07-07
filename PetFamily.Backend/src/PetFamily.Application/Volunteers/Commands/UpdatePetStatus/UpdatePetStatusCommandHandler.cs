using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.UpdatePetStatus
{
    public class UpdatePetStatusCommandHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
    {
        private readonly IValidator<UpdatePetStatusCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdatePetStatusCommandHandler> _logger;

        public UpdatePetStatusCommandHandler(
            IValidator<UpdatePetStatusCommand> validator,
            IVolunteersRepository volunteersRepository,
            ILogger<UpdatePetStatusCommandHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdatePetStatusCommand command, 
            CancellationToken cancelationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancelationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancelationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;
            var petId = PetId.Create(command.PetId);
            var status = PetStatus.Create(command.Status).Value;

            var updateStatusResult = volunteer.UpdatePetStatus(petId, status);
            if(updateStatusResult.IsFailure)
                return updateStatusResult.Error.ToErrorList();

            await _volunteersRepository.Save(volunteer, cancelationToken);

            _logger.LogInformation(
                "Pet status with ID={petId} updated by volunteer with ID={volunteerId}",
                petId.Value,
                volunteerId.Value
            );

            return petId.Value;
        }
    }
}
