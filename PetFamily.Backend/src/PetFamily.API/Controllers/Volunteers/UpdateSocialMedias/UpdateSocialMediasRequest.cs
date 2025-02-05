using PetFamily.Application.Volunteers.Shared;
using PetFamily.Application.Volunteers.UpdateSocialMedias;

namespace PetFamily.API.Controllers.Volunteers.UpdateSocialMedias
{
    public record UpdateSocialMediasRequest
    {
        public IEnumerable<SocialMediaDto> SocialMedias { get; }

        public UpdateSocialMediasRequest(IEnumerable<SocialMediaDto> socialMedias)
        {
            SocialMedias = socialMedias;
        }

        public UpdateSocialMediasCommand ToCommand(Guid id)
        {
            var socialMedias = SocialMedias.Select(sm => new SocialMediaInfo(sm.Link, sm.Title));
            var command = new UpdateSocialMediasCommand(id, socialMedias);

            return command;
        }
    }
}
