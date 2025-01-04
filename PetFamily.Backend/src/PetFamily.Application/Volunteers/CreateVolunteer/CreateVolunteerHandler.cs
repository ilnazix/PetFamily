using CSharpFunctionalExtensions;
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

        public async Task<Result<Guid, Error>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.NewVolunteerId();

            var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName);

            if (fullName.IsFailure)
            {
                return fullName.Error;
            }

            var emailResult = Email.Create(command.Email);

            if (emailResult.IsFailure)
            {
                return emailResult.Error;
            }

            var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);

            if (phoneNumberResult.IsFailure)
            {
                return phoneNumberResult.Error;
            }

            var volunteer = new Volunteer(volunteerId, fullName.Value, emailResult.Value, phoneNumberResult.Value);

            var socialMediasResults = command.SocialMedias.Select(sm => SocialMedia.Create(sm.Link, sm.Title));

            if(socialMediasResults.Any(sm => sm.IsFailure))
            {
                return socialMediasResults.First(sm => sm.IsFailure).Error;
            }

            volunteer.AddSocialMedias(socialMediasResults.Select(sm => sm.Value));

            var requiesitesResults = command.Requisites.Select(r => Requisite.Create(r.Title, r.Description));

            if (requiesitesResults.Any(r => r.IsFailure))
            {
                return requiesitesResults.First(r => r.IsFailure).Error;
            }

            volunteer.AddRequisites(requiesitesResults.Select(r => r.Value));

            var volunteerGuid = await _volunteersRepository.Add(volunteer, cancellationToken);

            return volunteerGuid;
        }
    }
}
