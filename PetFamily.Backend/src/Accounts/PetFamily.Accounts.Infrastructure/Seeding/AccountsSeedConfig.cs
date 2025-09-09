namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeedConfig
{
    public Dictionary<string, string[]> Roles { get; set; } = [];
    public Dictionary<string, string[]> Permissions { get; set; } = [];
}
