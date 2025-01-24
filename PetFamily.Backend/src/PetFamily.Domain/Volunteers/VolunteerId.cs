using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers
{
    public class VolunteerId : ComparableValueObject
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static VolunteerId NewVolunteerId() => new VolunteerId(Guid.NewGuid());
        public static VolunteerId Empty() => new VolunteerId(Guid.Empty);
        public static VolunteerId Create(Guid id) => new VolunteerId(id);

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(VolunteerId id) => id.Value; 
    }
}