using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;


namespace PetFamily.Application.Volunteers.Commands.Create
{
    public class CreateVolunteerCommandHandler : ICommandHandler<Guid, CreateVolunteerCommand>
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<CreateVolunteerCommand> _validator;
        private readonly ILogger<CreateVolunteerCommandHandler> _logger;

        public CreateVolunteerCommandHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<CreateVolunteerCommand> validator,
            ILogger<CreateVolunteerCommandHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
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

            var volunteerGuid = await _volunteersRepository.Add(volunteer, cancellationToken);

            _logger.LogInformation("Created new volunteer {firstName} {lastName} {middleName}. (Id = {id})",
                fullName.FirstName, fullName.LastName, fullName.MiddleName, volunteerGuid);

            return volunteerGuid;
        }
    }
}
