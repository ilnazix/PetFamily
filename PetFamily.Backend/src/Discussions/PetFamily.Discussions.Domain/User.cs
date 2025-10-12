using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Domain;

public class User 
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;

    private User() { }
    private User(
        Guid id, 
        string email)
    {
        Id = id;
        Email = email;
    }

    public static Result<User, Error> Create(
        Guid id, 
        string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Errors.General.ValueIsRequired(nameof(email));

        if (Guid.Empty == id)
            return Errors.General.ValueIsRequired(nameof(id));

        return new User(id, email);
    }
}

