using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.IntegrationTests.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.DeletePermanently;

namespace PetFamily.Application.IntegrationTests.Volunteers
{
    public class DeleteVolunteerTests : VolunteerBaseIntegrationTest
    {
        private readonly ICommandHandler<Guid, DeleteVolunteerPermanentlyCommand> _sut;

        public DeleteVolunteerTests(VolunteerIntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _sut = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeleteVolunteerPermanentlyCommand>>();
        }


        [Fact]
        public async Task Handle_ShouldDeleteVolunteer_WhenVolunteerExists()
        {
            // Arrange
            var volunteerId = await CreateVolunteer();
            var deleteVolunteerCommand = _fixture.BuildDeleteVolunteerPermanentlyCommand(volunteerId);

            // Act
            await _sut.Handle(deleteVolunteerCommand);

            // Assert
            var volunteer = _readDbContext
                .Volunteers
                .FirstOrDefault(v => v.Id == volunteerId);

            volunteer.Should().BeNull();
        }

        private async Task<Guid> CreateVolunteer()
        {
            var createVolunteerCommand = _fixture.BuildCreateVolunteerCommand();

            var createVolunteerCommandHandler = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();

            var volunteerResult = await createVolunteerCommandHandler.Handle(createVolunteerCommand);

            return volunteerResult.Value;
        }
    }
}
