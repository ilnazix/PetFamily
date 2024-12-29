namespace PetFamily.Domain.Species
{
    public record BreedId : IComparable<BreedId>
    {
        private BreedId(Guid value)
        {
            Value = value;
        }

        Guid Value { get; }

        public static BreedId NewBreedId => new BreedId(Guid.NewGuid());
        public static BreedId Empty => new BreedId(Guid.Empty);

        public int CompareTo(BreedId? other)
        {
            return Value.CompareTo(other?.Value);
        }
    }
}