using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class VolunteerId : ComparableValueObject
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }

        Guid Value { get; }

        public static VolunteerId NewVolunteerId => new VolunteerId(Guid.NewGuid());
        public static VolunteerId Empty => new VolunteerId(Guid.Empty);

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }
    }
}