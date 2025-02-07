using PetFamily.Application.Volunteers.Shared;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteers.UpdateMainInfo
{
    public record UpdateMainInfoRequest
    {
        public string FirstName { get; }
        public string LastName { get; } 
        public string MiddleName { get; }
        public int Experience { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        string Description { get; }

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

        public UpdateMainInfoCommand ToCommand(Guid id)
        {
            var fullName = new FullNameDto(FirstName, LastName, MiddleName);
            var command = new UpdateMainInfoCommand(
                id, 
                fullName,
                Experience,
                PhoneNumber,
                Email,
                Description);

            return command;
        }
    }
}
