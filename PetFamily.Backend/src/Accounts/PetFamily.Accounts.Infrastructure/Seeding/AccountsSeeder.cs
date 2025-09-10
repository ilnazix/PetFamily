using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PetFamily.Accounts.Infrastructure.Seeding;

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
        using var scope = _serviceScopeFactory.CreateScope();

        var seederService = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();

        await seederService.SeedAsync();
    }
}
