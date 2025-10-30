using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Species.Domain.Models;

public class Species : Entity<SpeciesId>
{
    public const int SPECIES_TITLE_MAX_LENGTH = 100;

    private readonly List<Breed> _breeds = new();

    //ef core
    private Species(SpeciesId id) : base(id) { }

    public Species(SpeciesId id, string title) : base(id)
    {
        Title = title;
    }

    public string Title { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species, Error> Create(SpeciesId id, string speciesTitle)
    {
        if (!IsTitleValid(speciesTitle))
        {
            return Errors.General.ValueIsInvalid(nameof(speciesTitle));
        }

        return new Species(id, speciesTitle);
    }

    public UnitResult<Error> UpdateTitle(string title)
    {
        if (!IsTitleValid(title))
        {
            return Errors.General.ValueIsInvalid(nameof(title));
        }

        Title = title;

        return UnitResult.Success<Error>();
    }

    public Result<Breed, Error> GetBreedById(BreedId id)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == id);

        if (breed is null)
        {
            return Errors.General.NotFound(id.Value);
        }

        return breed;
    }


    public UnitResult<Error> AddBreed(Breed breed)
    {
        _breeds.Add(breed);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateBreedTitle(BreedId breedId, string title)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == breedId);

        if (breed is null)
            return Errors.General.NotFound(breedId.Value);

        var result = breed.UpdateTitle(title);

        return result;
    }

    public UnitResult<Error> DeleteBreedById(BreedId breedId)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == breedId);
        if (breed is null)
            return Errors.General.NotFound(breedId.Value);

        _breeds.Remove(breed);
        return UnitResult.Success<Error>();
    }
    private static bool IsTitleValid(string title)
    {
        return !(string.IsNullOrWhiteSpace(title) || title.Length > SPECIES_TITLE_MAX_LENGTH);
    }
}
