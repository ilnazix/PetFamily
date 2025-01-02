using CSharpFunctionalExtensions;
using PetFamily.Domain.Species;

namespace PetFamily.Domain.Volunteer
{
    public class PetType : ComparableValueObject
    {
        private PetType(SpeciesId speciesId, BreedId breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public SpeciesId SpeciesId { get; }
        public BreedId BreedId { get; }

        public static Result<PetType> Create(SpeciesId speciesId, BreedId breedId)
        {
            var errors = string.Empty;

            if (speciesId is null)
            {
                errors += "Species id must be provided\n";
            }

            if (breedId is null)
            {
                errors += "Breed id must be provided\n";
            }

            if (string.IsNullOrEmpty(errors))
            {
                return Result.Success(new PetType(speciesId!, breedId!));
            }

            return Result.Failure<PetType>(errors);
        }

        protected override IEnumerable<IComparable> GetComparableEqualityComponents()
        {
            yield return SpeciesId;
            yield return BreedId;
        }
    }
}
