namespace PetFamily.Domain.Volunteer
{
    public record PetId : IComparable<PetId>
    {
        private PetId(Guid value) {
            Value = value;
        }

        Guid Value { get; }

        public static PetId NewPetId => new PetId(Guid.NewGuid());
        public static PetId Empty => new PetId(Guid.Empty);

        public int CompareTo(PetId? other)
        {
            return Value.CompareTo(other?.Value);
        }
    }
}
