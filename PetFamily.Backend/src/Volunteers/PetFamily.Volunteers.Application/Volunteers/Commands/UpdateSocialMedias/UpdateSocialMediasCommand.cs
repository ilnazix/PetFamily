using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;


namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedias
{
    public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialMediaInfo> SocialMedias) : ICommand;
}
