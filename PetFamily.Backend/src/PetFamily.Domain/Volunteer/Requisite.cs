using CSharpFunctionalExtensions;

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

        public static Result<Requisite> Create(string title, string description)
        {
            string errors = string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                errors += "Requisites title cannot be empty\n";
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                errors += "Requisites description cannot be empty\n";
            }

            if (string.IsNullOrEmpty(errors))
            {
                return Result.Success<Requisite>(new Requisite(title, description));
            }

            return Result.Failure<Requisite>(errors);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Title;
            yield return Description;
        }
    }
}
