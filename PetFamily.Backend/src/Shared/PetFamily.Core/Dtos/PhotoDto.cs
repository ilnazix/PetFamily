namespace PetFamily.Core.Dtos
{
    public class PhotoDto
    {
        public string Path { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
