using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.IntegrationTests.Extensions;
using PetFamily.Application.IntegrationTests.Infrastructure;
using PetFamily.Application.Species.Commands.Create;

namespace PetFamily.Application.IntegrationTests.Species
{
    public class CreateSpeciesTests : BaseIntegrationTest
    {
        private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;

        public CreateSpeciesTests(IntegrationTestWebAppFactory factory) 
            : base(factory)
        {
            _sut = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
        }

        [Fact]
        public async Task Handle_ShouldCreateSpecies_WhenCommandIsValid()
        {
            //Arrange
            var command = _fixture.BuildCreateSpeciesCommand();
            
            //Act
            var result = await _sut.Handle(command);

            //Assert
            var id = result.Value;
            var species = _readDbContext
                .Species
                .FirstOrDefault(s => s.Id == id);

            species.Should().NotBeNull();
        }
    }
}
