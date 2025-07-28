namespace PetFamily.Application.Providers
{
    public class FileData
    {
        public FileData(FileMetadata fileInfo, Stream content)
        {
            Info = fileInfo;
            Content = content;
        }

        public FileMetadata Info { get; init; }
        public Stream Content { get; init; } = null!;
    }

    public class FileMetadata
    {
        public FileMetadata(string bucketName, string path)
        {
            BucketName = bucketName;
            Path = path;
        }

        public string BucketName { get; init; }
        public string Path { get; init; }
    }
}
