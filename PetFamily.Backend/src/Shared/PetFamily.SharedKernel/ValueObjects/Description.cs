using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public class Description : ValueObject
{
    public const int DESCRIPTION_MAX_LENGTH = 2000;
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string description)
    {
        if (string.IsNullOrEmpty(description) || description.Length > DESCRIPTION_MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid(description);
        }

        return new Description(description);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}