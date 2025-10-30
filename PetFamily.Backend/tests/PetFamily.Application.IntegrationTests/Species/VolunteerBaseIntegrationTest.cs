using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application.Database;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Application.IntegrationTests.Species;

public class SpeciesBaseIntegrationTest : IClassFixture<SpeciesIntegrationTestWebAppFactory>, IAsyncLifetime
{
    protected readonly SpeciesIntegrationTestWebAppFactory _factory;
    protected readonly IServiceScope _scope;
    protected readonly Fixture _fixture;
    protected readonly ISpeciesReadDbContext _readDbContext;

    public SpeciesBaseIntegrationTest(SpeciesIntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _fixture = new Fixture();
        _readDbContext = _scope
            .ServiceProvider
            .GetRequiredService<ISpeciesReadDbContext>();
    }

    public Task DisposeAsync()
    {
        _scope.Dispose();

        return _factory.ResetDatabaseAsync();
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
