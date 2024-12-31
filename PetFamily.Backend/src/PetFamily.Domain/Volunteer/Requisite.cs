using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class Requisite : ComparableValueObject
    {
        string Title { get;  }
        string Description { get; }

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
                errors += "Название реквизита не может быть пустым\n";
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                errors += "Описание реквизита не может быть пустым\n";
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
