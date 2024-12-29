namespace PetFamily.Domain.Volunteer
{
    public record VolunteerId : IComparable<VolunteerId>
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }

        Guid Value { get; }

        public static VolunteerId NewPetId => new VolunteerId(Guid.NewGuid());
        public static VolunteerId Empty => new VolunteerId(Guid.Empty);

        public int CompareTo(VolunteerId? other)
        {
            return Value.CompareTo(other?.Value);
        }
    }
}