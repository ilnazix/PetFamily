using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species
{
    public class Breed : Entity<BreedId>
    {
        private Breed(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public static Result<Breed, Error> Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Errors.General.ValueIsRequired(nameof(title));
            }

            return new Breed(title);
        }
    }
}
