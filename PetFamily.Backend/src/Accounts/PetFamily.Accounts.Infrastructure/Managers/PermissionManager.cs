using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Commands;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

internal class PermissionManager : IPermissionManager
{
    private readonly AccountsWriteDbContext _dbContext;

    public PermissionManager(AccountsWriteDbContext dbContext)
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

    public async Task<List<string>> GetUserPermissionCodes(Guid userId)
    {
        var user = await _dbContext.Users
       .Include(u => u.Roles)
           .ThenInclude(r => r.Permissions)
       .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new List<string>();

        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.Code)
            .Distinct()
            .ToList();

        return permissions;
    }
}
