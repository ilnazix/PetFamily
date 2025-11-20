namespace PetFamily.Volunteers.Contracts.Requests;

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
        string email
    )
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        PhoneNumber = phoneNumber;
        Email = email;
    }
};