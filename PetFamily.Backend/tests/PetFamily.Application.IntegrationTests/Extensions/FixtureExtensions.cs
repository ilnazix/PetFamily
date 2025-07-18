using AutoFixture;
using PetFamily.Application.Species.Commands.AddBreed;
using PetFamily.Application.Volunteers.Commands.AddPet;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.DeletePermanently;

namespace PetFamily.Application.IntegrationTests.Extensions
{
    public static class FixtureExtensions
    {
        private const string DEFAULT_PHONE_NUMBER = "+71112223334";
        private const string DEFAULT_EMAIL = "testuser@test.com";

        public static CreateVolunteerCommand BuildCreateVolunteerCommand(
            this IFixture fixture)
        {
            return fixture.Build<CreateVolunteerCommand>()
                .With(c => c.PhoneNumber, DEFAULT_PHONE_NUMBER)
                .With(c => c.Email, DEFAULT_EMAIL)
                .Create();
        }

        public static DeleteVolunteerPermanentlyCommand BuildDeleteVolunteerPermanentlyCommand(
            this IFixture fixture,
            Guid volunteerId)
        {
            return fixture.Build<DeleteVolunteerPermanentlyCommand>()
                .With(c => c.Id, volunteerId)
                .Create();
        }

        public static AddPetCommand BuildAddPetCommand(
            this IFixture fixture, 
            Guid volunteerId,
            Guid speciesId,
            Guid breedId)
        {
            return fixture
                .Build<AddPetCommand>()
                .With(c => c.VolunteerId, volunteerId)
                .With(c => c.PhoneNumber, DEFAULT_PHONE_NUMBER)
                .With(c => c.SpeciesId, speciesId)
                .With(c => c.PetStatus, "needshelp")
                .With(c => c.BreeedId, breedId)
                .Create();
        }

        public static AddBreedCommand BuildAddBreedCommand(
            this IFixture fixture,
            Guid speciesId)
        {
             return fixture.Build<AddBreedCommand>()
                .With(c => c.SpeciesId, speciesId)
                .Create();
        }
    }
}
