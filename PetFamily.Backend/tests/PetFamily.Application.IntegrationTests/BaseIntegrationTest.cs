using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;

namespace PetFamily.Application.IntegrationTests
{
    public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
    {
        protected readonly IntegrationTestWebAppFactory _factory;
        protected readonly IServiceScope _scope;
        protected readonly Fixture _fixture;
        protected readonly IReadDbContext _readDbContext;

        public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _fixture = new Fixture();
            _readDbContext = _scope
                .ServiceProvider
                .GetRequiredService<IReadDbContext>();
        }

        public Task DisposeAsync()
        {
            _scope.Dispose();

            return _factory.ResetDatabaseAsync();
        }

        public Task InitializeAsync() => Task.CompletedTask;
    }
}
