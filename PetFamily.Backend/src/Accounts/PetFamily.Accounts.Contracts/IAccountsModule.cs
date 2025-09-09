namespace PetFamily.Accounts.Contracts;

public interface IAccountsModule
{
    Task<List<string>> GetUserPermissionCodes(Guid userId);
}
