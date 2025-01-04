using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Species
{
    public class Species : Entity<SpeciesId>
    {
        private readonly List<Breed> _breeds = new();

        public Species(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
        public IReadOnlyList<Breed> Breeds => _breeds;

        public static Result<Species, Error> Create(string title) 
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Errors.General.ValueIsRequired(nameof(title));
            }

            return new Species(title);
        }
    }
}
