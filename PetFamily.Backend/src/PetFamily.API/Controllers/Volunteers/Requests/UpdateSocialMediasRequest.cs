using PetFamily.Application.Volunteers.Commands.Shared;
using PetFamily.Application.Volunteers.Commands.UpdateSocialMedias;

namespace PetFamily.API.Controllers.Volunteers.Requests
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
