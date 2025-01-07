namespace PetFamily.Domain.Shared
{
    public record Error
    {
        public const string SEPARATOR = "||";

        public string Code {  get;  }
        public string Message { get;  }
        public ErrorType Type { get; }
        private Error(string code, string message, ErrorType type)
        {
            Code = code;
            Message = message;
            Type = type;
        }

        public static Error Validation(string code, string message) => new Error(code, message, ErrorType.Validation);
        public static Error NotFound(string code, string message) => new Error(code, message, ErrorType.NotFound);
        public static Error Failure(string code, string message) => new Error(code, message, ErrorType.Failure);
        public static Error Conflict(string code, string message) => new Error(code, message, ErrorType.Conflict);

        public string Serialize()
        {
            return string.Join(SEPARATOR, Code, Message, Type);
        }

        public static Error Deserialize(string error)
        {
            var parts = error.Split(SEPARATOR);

            if(parts.Length < 3)
            {
                throw new InvalidOperationException("Invalid serialized format");
            }

            if (!Enum.TryParse<ErrorType>(parts[2], out var type))
            {
                throw new InvalidOperationException("Invalid serialized format");
            }

            return new Error(parts[0], parts[1], type);
        }
    }
}
