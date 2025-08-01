namespace PetFamily.Volunteers.Contracts.Requests
{
    public record SetPetMainPhotoRequest
    {
        public string ImagePath { get; init; } = string.Empty;
    }

}
