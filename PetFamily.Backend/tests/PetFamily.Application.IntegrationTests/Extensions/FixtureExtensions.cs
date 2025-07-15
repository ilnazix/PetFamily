using AutoFixture;
using PetFamily.Application.Volunteers.Commands.Create;

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
    }
}
