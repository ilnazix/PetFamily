using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Volunteers;


namespace PetFamily.Volunteers.Application.Volunteers.Commands.Create
{
    public class CreateVolunteerCommandHandler : ICommandHandler<Guid, CreateVolunteerCommand>
    {
        private readonly IVolunteersUnitOfWork _unitOfWork;
        private readonly IValidator<CreateVolunteerCommand> _validator;
        private readonly ILogger<CreateVolunteerCommandHandler> _logger;

        public CreateVolunteerCommandHandler(
            IVolunteersUnitOfWork unitOfWork,
            IValidator<CreateVolunteerCommand> validator,
            ILogger<CreateVolunteerCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            CreateVolunteerCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToErrorList();
            }


            var volunteerId = VolunteerId.NewVolunteerId();
            var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName, command.FullName.MiddleName).Value;
            var email = Email.Create(command.Email).Value;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;


            var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);


            var socialMedias = command.SocialMedias.Select(sm => SocialMedia.Create(sm.Link, sm.Title).Value);
            volunteer.UpdateSocialMedias(socialMedias);


            var requisites = command.Requisites.Select(r => Requisite.Create(r.Title, r.Description).Value);
            volunteer.UpdateRequisites(requisites);

            var volunteerGuid = await _unitOfWork.VolunteersRepository.Add(volunteer, cancellationToken);

            await _unitOfWork.Commit(cancellationToken);

            _logger.LogInformation("Created new volunteer {firstName} {lastName} {middleName}. (Id = {Id})",
                fullName.FirstName, fullName.LastName, fullName.MiddleName, volunteerGuid);

            return volunteerGuid;
        }
    }
}
