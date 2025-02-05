using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Shared;

namespace PetFamily.API.Controllers.Volunteers
{
    public record CreateVolunteerRequest
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string MiddleName { get; }
        public string PhoneNumber { get; }
        public string Email { get; }

        public CreateVolunteerRequest(
            string firstName,
            string lastName,
            string middleName,
            string phoneNumber,
            string email,
            IEnumerable<SocialMediaInfo> socialMedias,
            IEnumerable<RequisitesDto> requisites
        )
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            Email = email;
            SocialMedias = socialMedias;
            Requisites = requisites;
        }

        public IEnumerable<SocialMediaInfo> SocialMedias { get; }
        public IEnumerable<RequisitesDto> Requisites { get; }

        public CreateVolunteerCommand ToCommand()
        {
            var fullNameDto = new FullNameDto(FirstName, LastName, MiddleName);
            var command = new CreateVolunteerCommand(
                FullName: fullNameDto,
                PhoneNumber: PhoneNumber,
                Email: Email,
                SocialMedias: SocialMedias.Select(sm => new SocialMediaInfo(sm.Link, sm.Title)),
                Requisites: Requisites.Select(r => new RequisitesDto(r.Title, r.Description))
            );

            return command;
        }
    };

}