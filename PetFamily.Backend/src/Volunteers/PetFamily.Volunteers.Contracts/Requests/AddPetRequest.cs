namespace PetFamily.Volunteers.Contracts.Requests;

public record AddPetRequest
{
    public string PetName { get; init; }
    public string Description { get; init; }
    public string PhoneNumber { get; init; }
    public string PetStatus { get; init; }
    public Guid SpeciesId { get; init; }
    public Guid BreedId { get; init; }

    public AddPetRequest(
        string petName,
        string description,
        string phoneNumber,
        string petStatus,
        Guid speciesId,
        Guid breedId)
    {
        PetName = petName;
        Description = description;
        PhoneNumber = phoneNumber;
        PetStatus = petStatus;
        SpeciesId = speciesId;
        BreedId = breedId;
    }
}
