using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.IntegrationTests.Extensions;
using PetFamily.Application.Volunteers.Commands.Create;

namespace PetFamily.Application.IntegrationTests
{
    public class CreateVolunteerTests : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
    {
        private readonly Fixture _fixture;
        private readonly ICommandHandler<Guid, CreateVolunteerCommand> _sut;
        private readonly IReadDbContext _readDbContext;
        private readonly IServiceScope _scope;
        private readonly IntegrationTestWebAppFactory _factory;

        public CreateVolunteerTests(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;

            _scope = factory.Services.CreateScope();

            _readDbContext = _scope.ServiceProvider
                .GetRequiredService<IReadDbContext>();

            _sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_ShouldCreateVolunteer_WhenCommandIsValid()
        {
            //Arrange
            var command = _fixture.BuildCreateVolunteerCommand();

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            _scope.ServiceProvider
                .GetRequiredService<IReadDbContext>();

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = _readDbContext.Volunteers
                .FirstOrDefault(v => v.Id == result.Value);

            volunteer.Should().NotBeNull();
            volunteer.FirstName.Should().Be(command.FullName.FirstName);
        }

        public Task DisposeAsync()
        {
            _scope.Dispose();
            
            return _factory.ResetDatabaseAsync();
        }

        public Task InitializeAsync() => Task.CompletedTask;
    }
}