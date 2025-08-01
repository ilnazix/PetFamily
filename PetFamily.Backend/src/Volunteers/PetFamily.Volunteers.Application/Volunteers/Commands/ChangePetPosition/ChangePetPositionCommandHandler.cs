using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.ChangePetPosition
{
    public class ChangePetPositionCommandHandler : ICommandHandler<Guid, ChangePetPositionCommand>
    {
        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly IValidator<ChangePetPositionCommand> _validator;
        private readonly ILogger<ChangePetPositionCommandHandler> _logger;

        public ChangePetPositionCommandHandler(
            IVolunteersUnitOfWork unitOfWork,
            IValidator<ChangePetPositionCommand> validator,
            ILogger<ChangePetPositionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            ChangePetPositionCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _unitOfWork. VolunteersRepository.GetById(volunteerId);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.Create(command.PetId);
            var position = Position.Create(command.NewPosition).Value;

            var result = volunteerResult.Value.MovePet(petId, position);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _unitOfWork.Commit(cancellationToken);

            return petId.Value;
        }
    }
}
