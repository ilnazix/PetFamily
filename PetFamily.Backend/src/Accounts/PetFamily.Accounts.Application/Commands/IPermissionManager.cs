using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Commands;

public interface IPermissionManager
{
    Task AddRangeIfNotExistAsync(IEnumerable<string> permissions);
    Task<Permission?> FindByCodeAsync(string code);
    Task<List<string>> GetUserPermissionCodes(Guid userId);
}
