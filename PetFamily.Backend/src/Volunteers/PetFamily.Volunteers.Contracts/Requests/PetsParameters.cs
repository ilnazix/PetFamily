namespace PetFamily.Volunteers.Contracts.Requests
{
    public record PetsParameters : RequestParameters
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public Guid[]? VolunteerIds { get; init; }
        public Guid? SpeciesId { get; init; }
        public Guid? BreedId { get; init; }
        public string? Color { get; init; }
        public string? Status { get; init; }
        public string? Country { get; init; }
        public string? State { get; init; }
        public string? City { get; init; }
    }

}
