using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain;

public class VolunteerInfo : ValueObject
{
    public FullName FullName { get; set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Email Email { get; private set; }

    private VolunteerInfo() { } // для EF/сериализации

    private VolunteerInfo(FullName fullName, PhoneNumber phoneNumber, Email email)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public static Result<VolunteerInfo, Error> Create(FullName fullName, PhoneNumber phoneNumber, Email email)
    {
        return new VolunteerInfo(fullName, phoneNumber, email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FullName;
        yield return PhoneNumber;
        yield return Email;
    }
}