using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer
{
    public class Requisite : ComparableValueObject
    {
        public string Title { get;  }
        public string Description { get; }

        private Requisite(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public static Result<Requisite, Error> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Errors.General.ValueIsRequired(nameof(title));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Errors.General.ValueIsRequired(nameof(description));
            }

            return new Requisite(title, description);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Title;
            yield return Description;
        }
    }
}
