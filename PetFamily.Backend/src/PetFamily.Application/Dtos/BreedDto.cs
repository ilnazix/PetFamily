namespace PetFamily.Application.Dtos
{
    public class BreedDto 
    {
        public Guid Id { get; set; }
        public Guid SpeciesId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
