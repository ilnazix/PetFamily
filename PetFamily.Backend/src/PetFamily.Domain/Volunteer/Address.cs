using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class Address : ComparableValueObject
    {
        string Country { get; }
        string State { get; }
        string City { get; }
        string Street { get; }
        string HouseNumber { get; }
        int? ApartmentNumber { get; }

        private Address(string country, string state, string city, string street, string houseNumber, int? apartmentNumber)
        {
            Country = country;
            State = state;
            City = city;
            Street = street;
            HouseNumber = houseNumber;
            ApartmentNumber = apartmentNumber;
        }

        public static Result<Address> Create(string country, string state, string city, string street, string houseNumber, int? apartmentNumber)
        {
            string errors = string.Empty;

            if (string.IsNullOrWhiteSpace(country))
            {
                errors += "Country can not be empty\n";
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                errors += "State can not be empty\n";
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                errors += "City cannot be empty\n";
            }

            if (string.IsNullOrWhiteSpace(street))
            {
                errors += "Street cannot be empty\n";
            }

            if (string.IsNullOrWhiteSpace(houseNumber))
            {
                errors += "House number cannot be empty\n";
            }

            if(apartmentNumber < 0)
            {
                errors += "Appartment number cannot be less then 1\n";
            }

            if (string.IsNullOrEmpty(errors))
            {
                return Result.Success<Address>(new Address(country, state, city, street, houseNumber, apartmentNumber));
            }

            return Result.Failure<Address>(errors);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Country;
            yield return State;
            yield return City;
            yield return Street;
            yield return HouseNumber;
            yield return ApartmentNumber;
        }
    }
}
