using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;


namespace PetFamily.Domain.Volunteers
{
    public class Pet : SoftDeleteableEntity<PetId>
    {
        //ef core
        private Pet(PetId id) : base(id) 
        { 
        }

        public Pet(
            PetId id, 
            PetName name, 
            PetType petType, 
            Description description, 
            PhoneNumber ownerPhoneNumber, 
            PetStatus status) : base(id)
        {
            Name = name;
            PetType = petType;
            Description = description;
            OwnerPhoneNumber = ownerPhoneNumber;
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }

        public PetName Name { get; private set; }

        public PetType PetType {  get; private set; }
        
        public Position Position { get; private set; }

        public Description Description { get; private set; }

        public Color Color { get; private set; } = Color.DefaultColor;

        public MedicalInformation? MedicalInformation { get; private set; }

        public Address? Address { get; private set; }

        public PhoneNumber OwnerPhoneNumber { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public PetStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public IReadOnlyList<Requisite> Requisites { get; private set; } = new List<Requisite>();

        public IReadOnlyList<Photo> Photos { get; private set; } = new List<Photo>();

        internal UnitResult<Error> SetPosition(Position position)
        {
            Position = position;

            return UnitResult.Success<Error>();
        }

        internal UnitResult<Error> MoveForward()
        {
            var newPositionResult = Position.MoveForward();

            if (newPositionResult.IsFailure)
                return newPositionResult.Error;

            Position = newPositionResult.Value;

            return UnitResult.Success<Error>();
        }

        internal UnitResult<Error> MoveBack()
        {
            var newPositionResult = Position.MoveBack();

            if (newPositionResult.IsFailure)
                return newPositionResult.Error;

            Position = newPositionResult.Value;

            return UnitResult.Success<Error>();
        }

        internal UnitResult<Error> SetPhotos(IEnumerable<Photo> photos)
        {
            Photos = photos.ToList();

            return UnitResult.Success<Error>();
        }

        internal UnitResult<Error> UpdateInfo(
            PetName name,
            PetType petType,
            Description description,
            PhoneNumber ownerPhoneNumber,
            Color color,
            IReadOnlyList<Requisite> requisites,
            MedicalInformation medicalInformation,
            Address address,
            DateTime dateOfBirth
            )
        {
            Name = name;
            PetType = petType;
            Description = description;
            OwnerPhoneNumber = ownerPhoneNumber;
            Color = color;
            Requisites = requisites;
            MedicalInformation = medicalInformation;
            Address = address;
            DateOfBirth = dateOfBirth;

            return UnitResult.Success<Error>();
        }
    }
}
