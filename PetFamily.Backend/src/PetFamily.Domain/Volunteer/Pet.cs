using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class Pet : Entity<PetId>
    {
        private readonly List<Requisite> _requisites = new List<Requisite>();

        //ef core
        private Pet() { }

        private Pet(PetId id, string name, string type, string description, PhoneNumber ownerPhoneNumber, PetStatus status) : base(id)
        {
            Name = name;
            Type = type;
            Description = description;
            OwnerPhoneNumber = ownerPhoneNumber;
            Status = status;
        }

        public string Name { get; private set; } = string.Empty;

        public string Type { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public string Breed { get; private set; } = string.Empty;

        public string Color { get; private set; } = string.Empty;

        public MedicalInformation? MedicalInformation { get; private set; }

        public Address? Address { get; private set; }

        public PhoneNumber? OwnerPhoneNumber { get; private set; }

        public DateTime DateofBirth { get; private set; }

        public PetStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<Requisite> Requisites => _requisites;

        public static Result<Pet> Create(PetId id, string name, string type, string description, PhoneNumber ownerPhoneNumber, PetStatus status)
        {
            string errors = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                errors += "Pet's name cannot be empty\n";
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                errors += "Description cannot be empty\n";
            }
            
            if (string.IsNullOrWhiteSpace(type))
            {
                errors += "Type cannot be empty\n";
            }

            if (ownerPhoneNumber == null) 
            {
                errors += "Owner's phone number must be provided\n";
            }

            if (string.IsNullOrEmpty(errors))
            {
                return Result.Success<Pet>(new Pet(name, type, description, ownerPhoneNumber!, status));
            }

            return Result.Failure<Pet>(errors);
        }
    }
}
