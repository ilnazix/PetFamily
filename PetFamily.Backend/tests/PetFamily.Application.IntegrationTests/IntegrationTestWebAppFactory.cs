using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PetFamily.Infrastructure.DbContexts;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace PetFamily.Application.IntegrationTests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17")
            .WithDatabase("pet_fmily")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        private Respawner _respawner = default!;
        private DbConnection _dbConnection = default!;

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            var scope = Services.CreateScope();

            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            await InitializeRespawner();
        }

        private async Task InitializeRespawner()
        {
            await _dbConnection.OpenAsync();

            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions 
                {
                    DbAdapter = DbAdapter.Postgres,
                    SchemasToInclude = ["public"]
                });
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureDefaultServices);
        }

        private void ConfigureDefaultServices(IServiceCollection services)
        {
            var writeDbContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(ApplicationWriteDbContext));

            if(writeDbContext is not null)
                services.Remove(writeDbContext);

            var readDbContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(ApplicationReadDbContext));

            if (readDbContext is not null)
                services.Remove(readDbContext);

            services.AddDbContext<ApplicationWriteDbContext>(options =>
            {
                options
                    .UseNpgsql(_dbContainer.GetConnectionString())
                    .UseSnakeCaseNamingConvention();
            });

            services.AddDbContext<ApplicationReadDbContext>(options =>
            {
                options
                    .UseNpgsql(_dbContainer.GetConnectionString())
                    .UseSnakeCaseNamingConvention();
            });

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationWriteDbContext>();

            context.Database.Migrate();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
