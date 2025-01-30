namespace PetFamily.Domain.Shared
{
    public record Error
    {
        public const string SEPARATOR = "||";

        public string Code {  get;  }
        public string Message { get;  }
        public ErrorType Type { get; }
        public string? InvalidField { get; } = null;
        private Error(string code, string message, ErrorType type, string? invalidField = null)
        {
            Code = code;
            Message = message;
            Type = type;
            InvalidField = invalidField;
        }

        public static Error Validation(string code, string message, string? invalidField) => new Error(code, message, ErrorType.Validation, invalidField);
        public static Error NotFound(string code, string message) => new Error(code, message, ErrorType.NotFound);
        public static Error Failure(string code, string message) => new Error(code, message, ErrorType.Failure);
        public static Error Conflict(string code, string message) => new Error(code, message, ErrorType.Conflict);

        public string Serialize()
        {
            return string.Join(SEPARATOR, Code, Message, Type, InvalidField);
        }

        public static Error Deserialize(string error)
        {
            var parts = error.Split(SEPARATOR).Where(p => !string.IsNullOrEmpty(p)).ToArray();

            if (parts.Length < 3 || parts.Length > 4)
            {
                throw new InvalidOperationException("Invalid serialized format.");
            }

            if (!Enum.TryParse<ErrorType>(parts[2], out var type))
            {
                throw new InvalidOperationException("Invalid error type in serialized format.");
            }

            var code = parts[0];
            var message = parts[1];
            var invalidField = parts.Length == 4 ? parts[3] : null;

            return new Error(code, message, type, invalidField);
        }

        public ErrorList ToErrorList()
        {
            return new ErrorList([this]);
        }
    }
}
