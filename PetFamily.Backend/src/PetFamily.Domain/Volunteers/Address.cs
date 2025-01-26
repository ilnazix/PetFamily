using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System.Dynamic;

namespace PetFamily.Domain.Volunteers
{
    public class Address : ComparableValueObject
    {
        public const int FIELD_MAX_LENGTH = 50;

        public string Country { get; }
        public string State { get; }
        public string City { get; }
        public string Street { get; }
        public string HouseNumber { get; }
        public int? ApartmentNumber { get; }

        private Address() { }

        private Address(string country, string state, string city, string street, string houseNumber, int? apartmentNumber)
        {
            Country = country;
            State = state;
            City = city;
            Street = street;
            HouseNumber = houseNumber;
            ApartmentNumber = apartmentNumber;
        }

        public static Result<Address, Error> Create(string country, string state, string city, string street, string houseNumber, int? apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(country) || country.Length > FIELD_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(country));
            }

            if (string.IsNullOrWhiteSpace(state) || state.Length > FIELD_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(state));
            }

            if (string.IsNullOrWhiteSpace(city) || city.Length > FIELD_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(city));
            }

            if (string.IsNullOrWhiteSpace(street) || street.Length > FIELD_MAX_LENGTH)
            {
                return Errors.General.ValueIsRequired(nameof(state));
            }

            if (string.IsNullOrWhiteSpace(houseNumber) || houseNumber.Length > FIELD_MAX_LENGTH)
            {
                return Errors.General.ValueIsRequired(nameof(houseNumber));
            }

            if(apartmentNumber < 0)
            {
                return Errors.General.ValueIsInvalid(nameof(apartmentNumber));
            }

            return new Address(country, state, city, street, houseNumber, apartmentNumber);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Country;
            yield return State;
            yield return City;
            yield return Street;
            yield return HouseNumber;
            yield return ApartmentNumber!;
        }
    }
}
