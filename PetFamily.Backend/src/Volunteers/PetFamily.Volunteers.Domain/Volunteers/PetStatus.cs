using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.Volunteers
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

        public static Result<PetStatus, Error> Create(string petStatus)
        {
            if (string.IsNullOrWhiteSpace(petStatus))
            {
                return Errors.General.ValueIsRequired(nameof(petStatus));
            }

            petStatus = petStatus.Trim().ToLower();

            if (_all.Any(g => g.Value == petStatus) == false)
            {
                return Errors.General.ValueIsInvalid(nameof(petStatus));
            }

            return new PetStatus(petStatus);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return Value;
        }
    }

}
