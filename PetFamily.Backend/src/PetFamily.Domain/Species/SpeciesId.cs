namespace PetFamily.Domain.Species
{
    public record SpeciesId : IComparable<SpeciesId>
    {
        private SpeciesId(Guid value)
        {
            Value = value;
        }

        Guid Value { get; }

        public static SpeciesId NewSpeciesId => new SpeciesId(Guid.NewGuid());
        public static SpeciesId Empty => new SpeciesId(Guid.Empty);

        public int CompareTo(SpeciesId? other)
        {
            return Value.CompareTo(other?.Value);
        }
    }
}