using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.IntegrationTests.Extensions;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.DeletePermanently;

namespace PetFamily.Application.IntegrationTests
{
    public class DeleteVolunteerTests : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
    {
        private readonly IntegrationTestWebAppFactory _factory;
        private readonly IServiceScope _scope;
        private readonly ICommandHandler<Guid, DeleteVolunteerPermanentlyCommand> _sut;
        private readonly ICommandHandler<Guid, CreateVolunteerCommand> _createVolunteerCommand;
        private readonly IReadDbContext _readDbContext;
        private readonly Fixture _fixture;

        public DeleteVolunteerTests(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;

            _scope = factory.Services.CreateScope();

            _sut = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeleteVolunteerPermanentlyCommand>>();

            _createVolunteerCommand = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();

            _readDbContext = _scope
                .ServiceProvider
                .GetRequiredService<IReadDbContext>();

            _fixture = new Fixture();
        }


        [Fact]
        public async Task Handle_ShouldDeleteVolunteer_WhenVolunteerExists()
        {
            // Arrange
            var createVolunteerCommand = _fixture.BuildCreateVolunteerCommand();

            var createVolunteerResult = await _createVolunteerCommand.Handle(createVolunteerCommand);

            var volunteerId = createVolunteerResult.Value;
            var deleteVolunteerCommand = _fixture.BuildDeleteVolunteerPermanentlyCommand(volunteerId);

            // Act
            await _sut.Handle(deleteVolunteerCommand);

            // Assert
            var volunteer = _readDbContext
                .Volunteers
                .FirstOrDefault(v => v.Id == volunteerId);

            volunteer.Should().BeNull();
        }

        public Task DisposeAsync()
        {
            _scope.Dispose();

            return _factory.ResetDatabaseAsync();
        }

        public Task InitializeAsync() => Task.CompletedTask;
    }
}
