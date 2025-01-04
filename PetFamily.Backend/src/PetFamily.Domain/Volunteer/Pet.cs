using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer
{
    public class Pet : Entity<PetId>
    {
        //ef core
        private Pet() { }

        private Pet(PetId id, string name, PetType petType, string description, PhoneNumber ownerPhoneNumber, PetStatus status) : base(id)
        {
            Name = name;
            PetType = petType;
            Description = description;
            OwnerPhoneNumber = ownerPhoneNumber;
            Status = status;
        }

        public string Name { get; private set; } = string.Empty;

        public PetType PetType {  get; private set; }         

        public string Description { get; private set; } = string.Empty;

        public string Color { get; private set; } = string.Empty;

        public MedicalInformation? MedicalInformation { get; private set; }

        public Address? Address { get; private set; }

        public PhoneNumber OwnerPhoneNumber { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public PetStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public RequisitesList? RequisitesList { get; private set; }

        public static Result<Pet, Error> Create(PetId id, string name, PetType petType, string description, PhoneNumber ownerPhoneNumber, PetStatus status)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Errors.General.ValueIsRequired(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Errors.General.ValueIsRequired(nameof(description));
            }

            return new Pet(id, name, petType, description, ownerPhoneNumber, status);
        }
    }
}
