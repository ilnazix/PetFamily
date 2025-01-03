using CSharpFunctionalExtensions;
using PetFamily.Domain.Volunteer;
using System.Net;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;

        public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
        {
            _volunteersRepository = volunteersRepository;
        }

        public async Task<Result<Guid, string>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken = default)
        {
            //создать доменную модель
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

            var volunteerGuid = await _volunteersRepository.Add(volunteer, cancellationToken);

            return volunteerGuid;
        }
    }
}
