using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;


namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedias
{
    public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialMediaInfo> SocialMedias) : ICommand;
}
