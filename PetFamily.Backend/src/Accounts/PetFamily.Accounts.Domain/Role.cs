using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public List<Permission> Permissions { get; set; } = [];
}
