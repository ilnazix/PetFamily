using PetFamily.Domain.Shared;

namespace PetFamily.API.Response
{
    public record Envelope
    {
        public object? Result { get; }
        public string? ErrorCode { get; }
        public string? ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        private Envelope(object? result, Error? error)
        {
            Result = result;
            ErrorCode = error?.Code;
            ErrorMessage = error?.Message;
            TimeGenerated = DateTime.UtcNow;
        }

        public static Envelope Error(Error error) => new(null, error);
        public static Envelope Ok(object? result) => new(result, null);
    }
}
