using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species
{
    public class Breed : Entity<BreedId>
    {
        public const int MAX_BREED_TITLE_LENGTH = 100;
        private Breed(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public static Result<Breed, Error> Create(string breedTitle)
        {
            if (string.IsNullOrWhiteSpace(breedTitle) || breedTitle.Length > MAX_BREED_TITLE_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(breedTitle));
            }

            return new Breed(breedTitle);
        }
    }
}
