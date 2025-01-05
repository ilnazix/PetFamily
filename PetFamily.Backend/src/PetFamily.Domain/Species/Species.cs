using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species
{
    public class Species : Entity<SpeciesId>
    {
        public const int SPECIES_TITLE_MAX_LENGTH = 100;

        private readonly List<Breed> _breeds = new();

        public Species(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
        public IReadOnlyList<Breed> Breeds => _breeds;

        public static Result<Species, Error> Create(string speciesTitle) 
        {
            if (string.IsNullOrWhiteSpace(speciesTitle) || speciesTitle.Length > SPECIES_TITLE_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(speciesTitle));
            }

            return new Species(speciesTitle);
        }
    }
}
