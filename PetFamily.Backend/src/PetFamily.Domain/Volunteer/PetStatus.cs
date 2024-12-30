using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class PetStatus 
    {
        public static readonly PetStatus NeedsHelp = new(nameof(NeedsHelp));
        public static readonly PetStatus SearchingForHome = new(nameof(SearchingForHome));
        public static readonly PetStatus FoundHome = new(nameof(FoundHome));

        private static readonly PetStatus[] _all = [NeedsHelp, SearchingForHome, FoundHome];

        public string Value { get; set; }

        private PetStatus(string value)
        {
            Value = value;
        }

        public static Result<PetStatus> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<PetStatus>("Value cannot be empty");
            }

            if(_all.Any(g => g.Value.Equals(value, StringComparison.OrdinalIgnoreCase)) == false)
            {
                return Result.Failure<PetStatus>("Value is invalid");
            }

            return Result.Success(new PetStatus(value));
        }
    }

}
