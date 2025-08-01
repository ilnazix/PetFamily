namespace PetFamily.Volunteers.Contracts.Requests
{
    public record UpdateMainInfoRequest
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string MiddleName { get; }
        public int Experience { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public string Description { get; }

        public UpdateMainInfoRequest(
            string firstName,
            string lastName,
            string middleName,
            int experience,
            string phoneNumber,
            string email,
            string description)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Experience = experience;
            PhoneNumber = phoneNumber;
            Email = email;
            Description = description;
        }
    }
}
