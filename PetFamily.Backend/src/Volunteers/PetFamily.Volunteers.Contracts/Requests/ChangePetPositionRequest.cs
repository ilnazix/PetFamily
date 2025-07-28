namespace PetFamily.Volunteers.Contracts.Requests
{
    public record ChangePetPositionRequest
    {
        public int NewPosition { get; init; }
    }
}
