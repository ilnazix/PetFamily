using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer
{
    public class Address : ComparableValueObject
    {
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
            if (string.IsNullOrWhiteSpace(country))
            {
                return Errors.General.ValueIsRequired(nameof(country));
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                return Errors.General.ValueIsRequired(nameof(state));
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                return Errors.General.ValueIsRequired(nameof(city));
            }

            if (string.IsNullOrWhiteSpace(street))
            {
                return Errors.General.ValueIsRequired(nameof(state));
            }

            if (string.IsNullOrWhiteSpace(houseNumber))
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
