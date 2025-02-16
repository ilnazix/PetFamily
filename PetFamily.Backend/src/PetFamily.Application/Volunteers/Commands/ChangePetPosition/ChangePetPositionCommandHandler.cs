using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.ChangePetPosition
{
    public class ChangePetPositionCommandHandler : ICommandHandler<Guid, ChangePetPositionCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<ChangePetPositionCommand> _validator;
        private readonly ILogger<ChangePetPositionCommandHandler> _logger;

        public ChangePetPositionCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<ChangePetPositionCommand> validator,
            ILogger<ChangePetPositionCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
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
            var volunteerResult = await _volunteersRepository.GetById(volunteerId);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.Create(command.PetId);
            var position = Position.Create(command.NewPosition).Value;

            var result = volunteerResult.Value.MovePet(petId, position);

            if (result.IsFailure)
                return result.Error.ToErrorList();

            var saveResult = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
            if (saveResult.IsFailure)
                return saveResult.Error.ToErrorList();

            return petId.Value;
        }
    }
}
