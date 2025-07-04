using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<UpdateMainInfoCommand> _validator;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<UpdateMainInfoCommand> validator,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateMainInfoCommand command,
            CancellationToken cancellationToken = default)
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

            var (firstName, lastName, middleName) = command.FullName;
            var fullName = FullName.Create(firstName, lastName, middleName).Value;

            var description = Description.Create(command.Description).Value;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
            var email = Email.Create(command.Email).Value;
            var experience = Experience.Create(command.Experience).Value;

            volunteerResult.Value.UpdateMainInfo(fullName, description, email, phoneNumber, experience);

            await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Volunteers's (Id={Id}) main info updated", volunteerResult.Value.Id.Value);

            return id.Value;
        }
    }
}
