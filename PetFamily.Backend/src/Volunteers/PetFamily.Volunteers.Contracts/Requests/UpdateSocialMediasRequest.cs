namespace PetFamily.Volunteers.Contracts.Requests
{
    public record UpdateSocialMediasRequest
    {
        public IEnumerable<SocialMediaDto> SocialMedias { get; }

        public UpdateSocialMediasRequest(IEnumerable<SocialMediaDto> socialMedias)
        {
            SocialMedias = socialMedias;
        }
    }
}
