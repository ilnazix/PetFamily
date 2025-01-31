namespace PetFamily.Application.Providers
{
    public class FileData
    {
        public FileData(string bucketName, string path, Stream content)
        {
            BucketName = bucketName;
            Path = path;
            Content = content;
        }

        public string BucketName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public Stream Content { get; set; } = null!;
    }
}
