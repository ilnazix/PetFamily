using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer;


namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;

        public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
        {
            _volunteersRepository = volunteersRepository;
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

            return volunteerGuid;
        }
    }
}
