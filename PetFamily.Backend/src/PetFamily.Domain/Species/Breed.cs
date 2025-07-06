using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species
{
    public class Breed : Entity<BreedId>
    {
        public const int MAX_BREED_TITLE_LENGTH = 100;

        //ef core
        private Breed(BreedId id) : base(id) { }
        private Breed(BreedId id, string title) : base(id)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public static Result<Breed, Error> Create(BreedId id, string breedTitle)
        {
            if (string.IsNullOrWhiteSpace(breedTitle) || breedTitle.Length > MAX_BREED_TITLE_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(breedTitle));
            }

            return new Breed(id, breedTitle);
        }

        internal UnitResult<Error> UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_BREED_TITLE_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(title));
            }

            Title = title;

            return UnitResult.Success<Error>();
        }
    }
}
