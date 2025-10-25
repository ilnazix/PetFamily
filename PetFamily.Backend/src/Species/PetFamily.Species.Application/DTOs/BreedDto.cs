namespace PetFamily.Species.Application.DTOs;

public class BreedDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public string Title { get; set; } = string.Empty;
}
