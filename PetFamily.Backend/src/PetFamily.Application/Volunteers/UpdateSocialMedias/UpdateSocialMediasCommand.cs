using PetFamily.Application.Volunteers.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialMedias
{
    public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialMediaInfo> SocialMedias);
}
