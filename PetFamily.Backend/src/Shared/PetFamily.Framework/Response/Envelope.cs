using PetFamily.SharedKernel;

namespace PetFamily.Framework.Response
{
    public record Envelope
    {
        public object? Result { get; }
        public ErrorList? Errors { get; }
        public DateTime TimeGenerated { get; }

        private Envelope(object? result, ErrorList? errors)
        {
            Result = result;
            Errors = errors;
            TimeGenerated = DateTime.UtcNow;
        }

        public static Envelope Error(ErrorList errors) => new(null, errors);
        public static Envelope Ok(object? result = null) => new(result, null);
    }
}
