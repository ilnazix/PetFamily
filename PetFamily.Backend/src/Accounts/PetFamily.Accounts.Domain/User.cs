using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    public VolunteerAccount? VolunteerAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    public AdminAccount? AdminAccount { get; set; }
    public IReadOnlyList<Role> Roles => _roles;

    private User() {}

    private List<Role> _roles = [];

    public static Result<User, Error> CreateAdmin(
        string email,
        string userName,
        Role role)
    {
        var isInvalidRole = !string.Equals(
            role.Name, 
            AdminAccount.ROLE, 
            StringComparison.InvariantCultureIgnoreCase);

        if (isInvalidRole)
            return Errors.User.InvalidRole();

        return new User
        {
            Email = email,
            UserName = userName,
            _roles = new List<Role> { role },
            AdminAccount = new()
        };
    }

    public static Result<User, Error> CreateParticipant(
        string email,
        string userName,
        Role role)
    {
        var isInvalidRole = !string.Equals(
            role.Name,
            ParticipantAccount.ROLE,
            StringComparison.InvariantCultureIgnoreCase);

        if (isInvalidRole)
            return Errors.User.InvalidRole();

        return new User
        {
            Email = email,
            UserName = userName,
            _roles = new List<Role> { role },
            ParticipantAccount = new()
        };
    }
}
