using CSharpFunctionalExtensions;
using System.Text.Json.Serialization;

namespace PetFamily.SharedKernel.ValueObjects
{
    public class Requisite : ComparableValueObject
    {
        public const int REQUISITE_TITLE_MAX_LENGTH = 100;
        public const int REQUISITE_DESCRIPTION_MAX_LENGTH = 2000;

        public string Title { get; }
        public string Description { get; }

        [JsonConstructor]
        private Requisite(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public static Result<Requisite, Error> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > REQUISITE_TITLE_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(title));
            }

            if (string.IsNullOrWhiteSpace(description) || description.Length > REQUISITE_DESCRIPTION_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(description));
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
