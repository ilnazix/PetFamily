namespace PetFamily.Application.Volunteers.Commands.AddPetPhoto
{
    public record UploadFileCommand(Stream Content, string FileName);
}