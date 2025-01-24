using CSharpFunctionalExtensions;
using PetFamily.Domain.Species;

namespace PetFamily.Domain.Volunteers
{
    public class PetType : ComparableValueObject
    {
        private PetType(SpeciesId speciesId, Guid breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public SpeciesId SpeciesId { get; }
        public Guid BreedId { get; }

        public static Result<PetType> Create(SpeciesId speciesId, Guid breedId)
        {
            return new PetType(speciesId, breedId);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return SpeciesId;
            yield return BreedId;
        }
    }
}
