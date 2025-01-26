using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System.Text.RegularExpressions;

namespace PetFamily.Domain.Volunteers
{
    public class PhoneNumber : ComparableValueObject
    {
        public string Value { get;  }

        private PhoneNumber(string value) 
        { 
            Value = value;
        }

        public static Result<PhoneNumber, Error> Create(string phoneNumber)
        {
            string pattern = @"^\+?\d{1,4}?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";
            
            if(!Regex.IsMatch(phoneNumber, pattern))
            {
                return Errors.General.ValueIsInvalid(nameof(phoneNumber));
            }

            return new PhoneNumber(phoneNumber);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }
    }
}
