using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects.Ids
{
    public class VolunteerRequestId : ComparableValueObject
    {
        private VolunteerRequestId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static VolunteerRequestId NewVolunteerId() => new VolunteerRequestId(Guid.NewGuid());
        public static VolunteerRequestId Empty() => new VolunteerRequestId(Guid.Empty);
        public static VolunteerRequestId Create(Guid id) => new VolunteerRequestId(id);

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(VolunteerRequestId id) => id.Value;
    }
}