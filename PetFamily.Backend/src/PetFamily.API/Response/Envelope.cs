namespace PetFamily.API.Response
{
    public record Envelope
    {
        public object? Result { get; }
        public List<ResponseError> Errors { get; }
        public DateTime TimeGenerated { get; }

        private Envelope(object? result, IEnumerable<ResponseError> errors)
        {
            Result = result;
            Errors = errors.ToList();
            TimeGenerated = DateTime.UtcNow;
        }

        public static Envelope Error(IEnumerable<ResponseError> errors) => new(null, errors);
        public static Envelope Ok(object? result) => new(result, []);
    }
}
