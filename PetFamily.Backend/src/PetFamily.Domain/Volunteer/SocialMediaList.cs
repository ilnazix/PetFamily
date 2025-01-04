using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class SocialMediaList : ValueObject
    {
        public IReadOnlyList<SocialMedia> SocialMedias { get; }

        public SocialMediaList() {}

        public SocialMediaList(List<SocialMedia> socialMedias)
        {
            SocialMedias = socialMedias;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach(var media in SocialMedias)
            {
                yield return media;
            }
        }
    }
}
