using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species
{
    public class Species : Entity<SpeciesId>
    {
        public const int SPECIES_TITLE_MAX_LENGTH = 100;

        private readonly List<Breed> _breeds = new();

        //ef core
        private Species(SpeciesId id) {}

        public Species(SpeciesId id,  string title) : base(id)
        {
            Title = title;
        }

        public string Title { get; private set; }
        public IReadOnlyList<Breed> Breeds => _breeds;

        public static Result<Species, Error> Create(SpeciesId id, string speciesTitle) 
        {
            if (string.IsNullOrWhiteSpace(speciesTitle) || speciesTitle.Length > SPECIES_TITLE_MAX_LENGTH)
            {
                return Errors.General.ValueIsInvalid(nameof(speciesTitle));
            }

            return new Species(id, speciesTitle);
        }

        public Result<Breed, Error> GetBreedById(BreedId id)
        {
            var breed = _breeds.FirstOrDefault(b => b.Id == id);

            if(breed is null)
            {
                return Errors.General.NotFound(id.Value);   
            }

            return breed;
        }
    }
}
