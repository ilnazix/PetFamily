using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Species
{
    public class SpeciesId : ComparableValueObject
    {
        private SpeciesId(Guid value)
        {
            Value = value;
        }

        Guid Value { get; }

        public static SpeciesId NewSpeciesId => new SpeciesId(Guid.NewGuid());
        public static SpeciesId Empty => new SpeciesId(Guid.Empty);

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }
    }
}