using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Accounts.Infrastructure.Options.Admin;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure.Seeding;

internal class AccountsSeederService 
{
    private readonly RoleManager<Role> _roleManager;
    private readonly PermissionManager _permissionManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly ILogger<AccountsSeederService> _logger;
    private readonly AdminOptions _options;

    public AccountsSeederService(
        RoleManager<Role> roleManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> options,
        ILogger<AccountsSeederService> logger)
    {
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _logger = logger;
        _options = options.Value;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Data seeding. . . "); 

        var json = await File.ReadAllTextAsync("etc/accounts.json");

        var config = JsonSerializer.Deserialize<AccountsSeedConfig>(json)
            ?? throw new ApplicationException("Could not deserialize accounts seeding config");

        await SeedPermissions(config);
        await SeedRoles(config);
        await SeedRolePermissions(config);
    }

    private async Task SeedRolePermissions(
       AccountsSeedConfig config)
    {
        foreach (var roleName in config.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            await _rolePermissionManager.AddRangeIfNotExistAsync(role!.Id, config.Roles[roleName]);
        }

        _logger.LogInformation("RolePermissions added to database");
    }

    private async Task SeedRoles(AccountsSeedConfig config)
    {
        foreach (var role in config.Roles.Keys)
        {
            var isRoleExist = await _roleManager.FindByNameAsync(role);

            if (isRoleExist is not null) continue;

            var newRole = new Role { Name = role };

            await _roleManager.CreateAsync(newRole);
        }

        _logger.LogInformation("Roles added to database");
    }

    private async Task SeedPermissions(AccountsSeedConfig config)
    {
        var permissionsToAdd = config
            .Permissions
            .SelectMany(p => p.Value);

        await _permissionManager.AddRangeIfNotExistAsync(permissionsToAdd);

        _logger.LogInformation("Permissions added to database");
    }
}
