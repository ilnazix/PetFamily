namespace PetFamily.Core.Models
{
    public record UploadFileCommand(Stream Content, string FileName);
}