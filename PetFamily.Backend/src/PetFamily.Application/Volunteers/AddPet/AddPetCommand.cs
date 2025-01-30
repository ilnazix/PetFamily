namespace PetFamily.Application.Volunteers.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        string PetName, 
        string Description, 
        string PhoneNumber,
        string PetStatus,
        Guid SpeciesId,
        Guid BreeedId
        ); 
}