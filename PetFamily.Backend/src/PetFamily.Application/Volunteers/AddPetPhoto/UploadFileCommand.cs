namespace PetFamily.Application.Volunteers.AddPetPhoto
{
    public record UploadFileCommand(Stream Content, string FileName);
}