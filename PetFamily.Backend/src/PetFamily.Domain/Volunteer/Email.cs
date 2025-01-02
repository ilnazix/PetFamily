using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetFamily.Domain.Volunteer
{
    public class Email : ComparableValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if(Regex.IsMatch(email, pattern))
            {
                return Result.Success<Email>(new Email(email));
            }

            return Result.Failure<Email>("Email is not valid");
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }
    }
}
