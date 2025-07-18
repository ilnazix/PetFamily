using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.IntegrationTests.Extensions;
using PetFamily.Application.IntegrationTests.Infrastructure;
using PetFamily.Application.Species.Commands.AddBreed;
using PetFamily.Application.Species.Commands.Create;
using PetFamily.Application.Volunteers.Commands.AddPet;
using PetFamily.Application.Volunteers.Commands.Create;

namespace PetFamily.Application.IntegrationTests.Volunteers
{
    public class AddPetTests : BaseIntegrationTest
    {
        private readonly ICommandHandler<Guid, AddPetCommand> _sut;

        public AddPetTests(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _sut = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
        }

        [Fact]
        public async Task Handle_ShouldAddPet_WhenCommandIsValid()
        {
            //Arrange
            var volunteerId = await CreateVolunteer();
            var speciesId = await CreateSpecies();
            var breedId = await CreateBreed(speciesId);

            var createPetCommand = _fixture.BuildAddPetCommand(volunteerId, speciesId, breedId);

            //Act
            var createPetResult = await _sut.Handle(createPetCommand);

            //Assert
            var petId = createPetResult.Value;

            var pet = _readDbContext
                .Pets
                .FirstOrDefault(pet => pet.Id == petId);

            pet.Should().NotBeNull();
            pet.Id.Should().Be(petId);
            pet.Name.Should().Be(createPetCommand.PetName);
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

        private async Task<Guid> CreateSpecies()
        {
            var createSpeciesCommand = _fixture
                .Build<CreateSpeciesCommand>()
                .Create();

            var createSpeciesCommandHandler = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();

            var speciesResult = await createSpeciesCommandHandler.Handle(createSpeciesCommand);

            return speciesResult.Value;
        }

        private async Task<Guid> CreateBreed(Guid speciesId)
        {
            var createBreedCommand = _fixture.BuildAddBreedCommand(speciesId);

            var createBreedCommandHandler = _scope
                .ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();

            var breedResult = await createBreedCommandHandler.Handle(createBreedCommand);

            return breedResult.Value;
        }
    }
}
