using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer
{
    public class SocialMedia : ComparableValueObject
    {
        public string Link { get; }
        public string Title { get; }

        private SocialMedia(string link, string title)
        {
            Link = link;
            Title = title;
        }

        public static Result<SocialMedia, Error> Create(string link, string title)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return Errors.General.ValueIsRequired(nameof(link));
            }
            
            if (string.IsNullOrWhiteSpace(title))
            {
                return Errors.General.ValueIsRequired(nameof(title));
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
