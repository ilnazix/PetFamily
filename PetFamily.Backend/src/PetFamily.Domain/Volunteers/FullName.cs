using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers
{
    public class FullName : ComparableValueObject
    {
        public const int NAME_MAX_LENGTH = 50;

        public string FirstName { get;  } 
        public string LastName { get; } 
        public string MiddleName { get; }

        private FullName(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public static Result<FullName, Error> Create(string firstName, string lastName, string middleName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > NAME_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > NAME_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(lastName));
            }

            if (string.IsNullOrWhiteSpace(middleName) || middleName.Length > NAME_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(middleName));
            }

            return new FullName(firstName, lastName, middleName);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;  
            yield return MiddleName;
        }
    }
}
