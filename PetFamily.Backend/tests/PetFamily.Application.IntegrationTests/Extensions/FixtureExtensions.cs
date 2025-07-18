using AutoFixture;
using PetFamily.Application.Volunteers.Commands.AddPet;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.DeletePermanently;

namespace PetFamily.Application.IntegrationTests.Extensions
{
    public static class FixtureExtensions
    {
        public static CreateVolunteerCommand BuildCreateVolunteerCommand(
            this IFixture fixture)
        {
            return fixture.Build<CreateVolunteerCommand>()
                .With(c => c.PhoneNumber, "+71112223334")
                .With(c => c.Email, "testuser@test.com")
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
    }
}
