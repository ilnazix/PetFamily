using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Managers;

internal class RolePermissionManager 
{
    private readonly AccountsDbContext _dbContext;

    public RolePermissionManager(AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRangeIfNotExistAsync(
        Guid roleId, 
        IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await _dbContext.Permissions
                .FirstAsync(p => p.Code == permissionCode);

            var rolePermissionExist = await _dbContext.RolePermissions
                .AnyAsync(
                    rp => rp.PermissionId == permission!.Id
                    && rp.RoleId == roleId);

            if (rolePermissionExist) continue;

            var newPermission = new RolePermission()
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            };

            await _dbContext.AddAsync(newPermission);
        }

        await _dbContext.SaveChangesAsync();
    }
}
