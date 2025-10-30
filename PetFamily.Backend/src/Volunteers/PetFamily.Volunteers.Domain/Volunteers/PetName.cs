using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.Volunteers;

public class PetName : ValueObject
{
    public const int PET_NAME_MAX_LENGTH = 100;
    public string Value { get; }

    private PetName(string value)
    {
        Value = value;
    }

    public static Result<PetName, Error> Create(string petName)
    {
        if (string.IsNullOrEmpty(petName) || petName.Length > PET_NAME_MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid(petName);
        }

        return new PetName(petName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}