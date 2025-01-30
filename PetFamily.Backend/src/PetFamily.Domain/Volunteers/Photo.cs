using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers
{
    public class Photo : ValueObject
    {
        public static readonly IReadOnlyList<string> ALLOWED_EXTENSIONS = [".jpg", ".jpeg", ".img"]; 

        public string Path { get; }
        public string FileName { get; }

        private Photo(string path, string fileName)
        {
            Path = path;
            FileName = fileName;
        }

        public static Result<Photo, Error> Create(string path, string fileName)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Errors.General.ValueIsInvalid(nameof(path));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return Errors.General.ValueIsInvalid(nameof(fileName));
            }

            var extension = System.IO.Path.GetExtension(fileName)?.ToLower();

            if (string.IsNullOrEmpty(extension) || !ALLOWED_EXTENSIONS.Contains(extension))
            {
                return Errors.General.ValueIsInvalid(nameof(extension));
            }

            return new Photo(path, fileName);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Path;
            yield return FileName;
        }
    }
}
