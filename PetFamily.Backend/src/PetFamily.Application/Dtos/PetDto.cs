namespace PetFamily.Application.Dtos
{
    public class PetDto
    {
        public Guid Id { get; init; }
        public Guid VolunteerId { get; set; }
        public string Name { get; init; } = string.Empty;
        public int Position { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime DateOfBirth { get; init; }
        public string Status { get; init; } = string.Empty;
        public string OwnerPhoneNumber { get; init; } = string.Empty;
        public RequisiteDto[] Requisites { get; init; } = [];
        public PhotoDto[] Photos { get; init; } = [];
        public string? Color { get; init; } = string.Empty;
        public bool? IsCastrated { get; init; }
        public bool? IsVaccinated { get; init;  }
        public string? HealthInformation { get; init; } = string.Empty;
        public int? Height { get; init; }
        public int? Weight { get; init; }
        public Guid SpeciesId { get; init; }
        public Guid BreedId { get; init; }
        public string? Country { get; init; } = string.Empty;
        public string? State { get; init; } = string.Empty;
        public string? City { get; init; } = string.Empty;
        public string? Street { get; init; } = string.Empty;
        public string? HouseNumber { get; } = string.Empty;
        public int? ApartmentNumber { get; }

    }
}
