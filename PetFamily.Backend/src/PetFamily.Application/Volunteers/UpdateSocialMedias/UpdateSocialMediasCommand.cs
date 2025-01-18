using PetFamily.Application.Volunteers.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialMedias
{
    public record UpdateSocialMediasCommand(Guid VolunteerId, UpdateSocialMediaDto Dto);
    
    public record UpdateSocialMediaDto(SocialMediaDto[] SocialMedias);
}
