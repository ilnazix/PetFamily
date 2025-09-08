using PetFamily.Accounts.Application.Commands;
using PetFamily.Accounts.Contracts;

namespace PetFamily.Accounts.Presentation;

internal class AccountsModule : IAccountsModule
{
    private readonly IPermissionManager _permissionManager;

    public AccountsModule(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }
    public Task<List<string>> GetUserPermissionCodes(Guid userId)
    {
        return _permissionManager.GetUserPermissionCodes(userId);
    }
}
