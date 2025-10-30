using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.IntegrationTests.Species;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Application.IntegrationTests.Volunteers;

public class VolunteerBaseIntegrationTest : IClassFixture<VolunteerIntegrationTestWebAppFactory>, IAsyncLifetime
{
    protected readonly VolunteerIntegrationTestWebAppFactory _factory;
    protected readonly IServiceScope _scope;
    protected readonly Fixture _fixture;
    protected readonly IVolunteersReadDbContext _readDbContext;

    public VolunteerBaseIntegrationTest(VolunteerIntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _fixture = new Fixture();
        _readDbContext = _scope
            .ServiceProvider
            .GetRequiredService<IVolunteersReadDbContext>();
    }

    public Task DisposeAsync()
    {
        _scope.Dispose();

        return _factory.ResetDatabaseAsync();
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
