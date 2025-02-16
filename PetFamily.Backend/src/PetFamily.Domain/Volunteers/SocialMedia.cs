using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System.Text.Json.Serialization;

namespace PetFamily.Domain.Volunteers
{
    public class SocialMedia : ComparableValueObject
    {
        public const int SOCIAL_MEDIA_LINK_MAX_LENGTH = 2000;
        public const int SOCIAL_MEDIA_TITLE_MAX_LENGTH = 100;

        public string Link { get; }
        public string Title { get; }

        [JsonConstructor]
        private SocialMedia(string link, string title)
        {
            Link = link;
            Title = title;
        }

        public static Result<SocialMedia, Error> Create(string link, string title)
        {
            if (string.IsNullOrWhiteSpace(link) || link.Length > SOCIAL_MEDIA_LINK_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(link));
            }
            
            if (string.IsNullOrWhiteSpace(title) || title.Length > SOCIAL_MEDIA_TITLE_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(title));
            }
            
            return new SocialMedia(link, title);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Link;
            yield return Title;
        }
    }
}
