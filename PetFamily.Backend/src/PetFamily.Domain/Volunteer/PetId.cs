using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class PetId : ComparableValueObject
    {
        private PetId(Guid value) {
            Value = value;
        }

        Guid Value { get; }

        public static PetId NewPetId => new PetId(Guid.NewGuid());
        public static PetId Empty => new PetId(Guid.Empty);

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
           yield return Value;
        }
    }
}
