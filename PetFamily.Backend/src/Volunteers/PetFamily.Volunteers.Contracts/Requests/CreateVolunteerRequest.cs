namespace PetFamily.Volunteers.Contracts.Requests
{
    public record CreateVolunteerRequest
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string MiddleName { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public IEnumerable<SocialMediaDto> SocialMedias { get; }
        public IEnumerable<RequisiteDto> Requisites { get; }

        public CreateVolunteerRequest(
            string firstName,
            string lastName,
            string middleName,
            string phoneNumber,
            string email,
            IEnumerable<SocialMediaDto> socialMedias,
            IEnumerable<RequisiteDto> requisites
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
    };

}