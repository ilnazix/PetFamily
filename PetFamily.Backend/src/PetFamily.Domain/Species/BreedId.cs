using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Species
{
    public class BreedId : ComparableValueObject
    {
        private BreedId(Guid value)
        {
            Value = value;
        }

        Guid Value { get; }

        public static BreedId NewBreedId => new BreedId(Guid.NewGuid());
        public static BreedId Empty => new BreedId(Guid.Empty);

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }
    }
}