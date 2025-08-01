using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
    {
        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateMainInfoCommand> _validator;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolunteersUnitOfWork unitOfWork,
            IValidator<UpdateMainInfoCommand> validator,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _unitOfWork = unitOfWork;
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
            var volunteerResult = await _unitOfWork.VolunteersRepository.GetById(id, cancellationToken);
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

            await _unitOfWork.Commit(cancellationToken);

            _logger.LogInformation("Volunteers's (Id={Id}) main info updated", volunteerResult.Value.Id.Value);

            return id.Value;
        }
    }
}
