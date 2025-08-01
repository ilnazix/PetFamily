using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Volunteers;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetStatus
{
    public class UpdatePetStatusCommandHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
    {
        private readonly IValidator<UpdatePetStatusCommand> _validator;
        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePetStatusCommandHandler> _logger;

        public UpdatePetStatusCommandHandler(
            IValidator<UpdatePetStatusCommand> validator,
            IVolunteersUnitOfWork volunteersRepository,
            ILogger<UpdatePetStatusCommandHandler> logger)
        {
            _validator = validator;
            _unitOfWork = volunteersRepository;
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
            var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(volunteerId, cancelationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var volunteer = volunteerResult.Value;
            var petId = PetId.Create(command.PetId);
            var status = PetStatus.Create(command.Status).Value;

            var updateStatusResult = volunteer.UpdatePetStatus(petId, status);
            if (updateStatusResult.IsFailure)
                return updateStatusResult.Error.ToErrorList();

            await _unitOfWork.Commit(cancelationToken);

            _logger.LogInformation(
                "Pet status with ID={petId} updated by volunteer with ID={volunteerId}",
                petId.Value,
                volunteerId.Value
            );

            return petId.Value;
        }
    }
}
