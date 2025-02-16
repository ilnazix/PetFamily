using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.Restore
{
    public class RestoreVolunteerCommandHandler : ICommandHandler<Guid, RestoreVolunteerCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<RestoreVolunteerCommand> _validator;
        private readonly ILogger<RestoreVolunteerCommandHandler> _logger;

        public RestoreVolunteerCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<RestoreVolunteerCommand> validator,
            ILogger<RestoreVolunteerCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            RestoreVolunteerCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToErrorList();
            }

            var id = VolunteerId.Create(command.Id);
            var volunteerResult = await _volunteersRepository.GetById(id, cancellationToken);

            if (volunteerResult.IsFailure)
            {
                return volunteerResult.Error.ToErrorList();
            }

            var volunteer = volunteerResult.Value;
            volunteer.Restore();

            await _volunteersRepository.Save(volunteer, cancellationToken);

            _logger.LogInformation("Restore volunteer with id={id}", id.Value);

            return id.Value;
        }
    }
}
