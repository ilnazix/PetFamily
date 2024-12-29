namespace PetFamily.Domain.Pet
{
    public record PetId
    {
        private PetId(Guid value) {
            Value = value;
        }

        Guid Value { get; }

        public static PetId NewPetId => new PetId(Guid.NewGuid());
        public static PetId Empty => new PetId(Guid.Empty);
    }
}
