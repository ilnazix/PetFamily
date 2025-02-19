using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.Shared;


namespace PetFamily.Application.Volunteers.Commands.UpdateSocialMedias
{
    public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialMediaInfo> SocialMedias) : ICommand;
}
