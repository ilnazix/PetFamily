using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Volunteer;


namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<CreateVolunteerHandler> _logger;

        public CreateVolunteerHandler(IVolunteersRepository volunteersRepository, ILogger<CreateVolunteerHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, ValidationResult>> Handle(
            CreateVolunteerCommand command,
            IValidator<CreateVolunteerCommand> validator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }


            var volunteerId = VolunteerId.NewVolunteerId();
            var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName).Value;
            var email = Email.Create(command.Email).Value;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;


            var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);


            var socialMedias = command.SocialMedias.Select(sm => SocialMedia.Create(sm.Link, sm.Title).Value);
            volunteer.AddSocialMedias(socialMedias);


            var requiesites = command.Requisites.Select(r => Requisite.Create(r.Title, r.Description).Value);
            volunteer.AddRequisites(requiesites);

            var volunteerGuid = await _volunteersRepository.Add(volunteer, cancellationToken);

            _logger.LogInformation("Created new volunteer {firstName} {lastName} {middleName}. (Id = {id})", 
                fullName.FirstName, fullName.LastName, fullName.MiddleName, volunteerGuid);

            return volunteerGuid;
        }
    }
}
