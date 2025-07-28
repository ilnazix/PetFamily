using CSharpFunctionalExtensions;


namespace PetFamily.SharedKernel.ValueObjects
{
    public class Photo : ValueObject
    {
        public static readonly IReadOnlyList<string> ALLOWED_EXTENSIONS = [".jpg", ".jpeg", ".img", ".png"];

        public string Path { get; }
        public string FileName { get; }
        public bool IsMain { get; }

        private Photo(string path, string fileName, bool isMain)
        {
            Path = path;
            FileName = fileName;
            IsMain = isMain;
        }

        public static Result<Photo, Error> Create(string path, string fileName, bool isMain)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Errors.General.ValueIsInvalid(nameof(path));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return Errors.General.ValueIsInvalid(nameof(fileName));
            }

            var extension = System.IO.Path.GetExtension(path)?.ToLower();

            if (string.IsNullOrEmpty(extension) || !ALLOWED_EXTENSIONS.Contains(extension))
            {
                return Errors.General.ValueIsInvalid(nameof(extension));
            }

            return new Photo(path, fileName, isMain);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Path;
            yield return FileName;
            yield return IsMain;
        }
    }
}
