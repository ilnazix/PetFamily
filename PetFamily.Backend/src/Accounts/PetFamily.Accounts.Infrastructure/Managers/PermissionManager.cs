using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Managers;

internal class PermissionManager
{
    private readonly AccountsDbContext _dbContext;

    public PermissionManager(AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Permission?> FindByCodeAsync(string code)
    {
        return _dbContext.Permissions
            .FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task AddRangeIfNotExistAsync(IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions) 
        {
            var isPermissionExist = await _dbContext
                        .Permissions
                        .AnyAsync(p => p.Code == permissionCode);

            if (isPermissionExist) continue;

            await _dbContext.Permissions.AddAsync(new Permission { Code = permissionCode });
        }

        await _dbContext.SaveChangesAsync();
    }
}
