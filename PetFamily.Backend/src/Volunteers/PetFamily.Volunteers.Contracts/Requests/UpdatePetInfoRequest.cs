namespace PetFamily.Volunteers.Contracts.Requests;

public record UpdatePetInfoRequest
{
    public string PetName { get; init; } = default!;
    public Guid SpeciesId { get; init; }
    public Guid BreedId { get; init; }
    public string Description { get; init; } = default!;
    public string OwnerPhoneNumber { get; init; } = default!;
    public string Color { get; init; } = default!;
    public DateTime DateOfBirth { get; init; }
    public IEnumerable<RequisiteDto> Requisites { get; init; } = Enumerable.Empty<RequisiteDto>();
    public bool IsCastrated { get; init; }
    public bool IsVaccinated { get; init; }
    public string HealthInformation { get; init; } = default!;
    public int Height { get; init; }
    public int Weight { get; init; }
    public string Country { get; init; } = default!;
    public string State { get; init; } = default!;
    public string City { get; init; } = default!;
    public string Street { get; init; } = default!;
    public string HouseNumber { get; init; } = default!;
    public int? ApartmentNumber { get; init; }
}
