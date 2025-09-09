using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Managers;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeedConfig
{
    public Dictionary<string, string[]> Roles { get; set; } = [];
    public Dictionary<string, string[]> Permissions { get; set; } = [];
}

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AccountsSeeder> _logger;

    public AccountsSeeder(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AccountsSeeder> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Data seeding. . . ");

        var json = await File.ReadAllTextAsync("etc/accounts.json");

        var config = JsonSerializer.Deserialize<AccountsSeedConfig>(json)
            ?? throw new ApplicationException("Could not deserialize accounts seeding config");

        using var scope = _serviceScopeFactory.CreateScope();

        var roleManager = scope
            .ServiceProvider
            .GetRequiredService<RoleManager<Role>>();

        var permissionManager = scope
            .ServiceProvider
            .GetRequiredService<PermissionManager>();

        var rolePermissionManager = scope
            .ServiceProvider
            .GetRequiredService<RolePermissionManager>();



        await SeedPermissions(config, permissionManager);
        await SeedRoles(config, roleManager);
        await SeedRolePermissions(config, roleManager, rolePermissionManager);
    }

    private async Task SeedRolePermissions(AccountsSeedConfig config, RoleManager<Role> roleManager, RolePermissionManager rolePermissionManager)
    {
        foreach (var roleName in config.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            await rolePermissionManager.AddRangeIfNotExistAsync(role!.Id, config.Roles[roleName]);
        }

        _logger.LogInformation("RolePermissions added to database");
    }

    private async Task SeedRoles(
        AccountsSeedConfig config, 
        RoleManager<Role> roleManager)
    {
        foreach (var role in config.Roles.Keys)
        {
            var isRoleExist = await roleManager.FindByNameAsync(role);

            if (isRoleExist is not null) continue;

            var newRole = new Role { Name = role };

            await roleManager.CreateAsync(newRole);
        }

        _logger.LogInformation("Roles added to database");
    }

    private async Task SeedPermissions(
        AccountsSeedConfig config, 
        PermissionManager permissionManager)
    {
        var permissionsToAdd = config
            .Permissions
            .SelectMany(p => p.Value);

        await permissionManager.AddRangeIfNotExistAsync(permissionsToAdd);

        _logger.LogInformation("Permissions added to database");
    }
}
