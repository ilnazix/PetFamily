using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Domain;

public class User 
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    private User() { }
    private User(
        Guid id, 
        string firstName, 
        string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<User, Error> Create(
        Guid id, 
        string firstName,
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired(nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired(nameof(lastName));

        if (Guid.Empty == id)
            return Errors.General.ValueIsRequired(nameof(id));

        return new User(id, firstName, lastName);
    }
}

