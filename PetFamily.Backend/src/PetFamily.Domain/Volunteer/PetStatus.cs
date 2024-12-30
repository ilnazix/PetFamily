using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public class PetStatus : ComparableValueObject
    {
        public static readonly PetStatus NeedsHelp = new(nameof(NeedsHelp).ToLower());
        public static readonly PetStatus SearchingForHome = new(nameof(SearchingForHome).ToLower());
        public static readonly PetStatus FoundHome = new(nameof(FoundHome).ToLower());

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

            value = value.Trim().ToLower();

            if(_all.Any(g => g.Value == value) == false)
            {
                return Result.Failure<PetStatus>("Value is invalid");
            }

            return Result.Success(new PetStatus(value));
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }
    }

}
