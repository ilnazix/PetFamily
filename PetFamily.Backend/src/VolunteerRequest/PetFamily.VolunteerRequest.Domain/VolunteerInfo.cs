using CSharpFunctionalExtensions;

namespace PetFamily.VolunteerRequest.Domain;

public class VolunteerInfo : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}

