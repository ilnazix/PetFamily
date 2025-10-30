using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.Volunteers;

public class MedicalInformation : ComparableValueObject
{
    public const int MAX_HEALTH_INFO_LENGTH = 2000;
    public bool IsCastrated { get; }
    public bool IsVaccinated { get; }
    public string HealthInformation { get; }
    public int Height { get; }
    public int Weight { get; }

    private MedicalInformation(string healthInformation, int height, int weight, bool isVaccinated, bool isCastrated)
    {
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HealthInformation = healthInformation;
        Height = height;
        Weight = weight;
    }

    public static Result<MedicalInformation, Error> Create(string healthInformation, int height, int weight, bool isVaccinated, bool isCastrated)
    {
        if (string.IsNullOrWhiteSpace(healthInformation) || healthInformation.Length > MAX_HEALTH_INFO_LENGTH)
        {
            return Errors.General.ValueIsInvalid(nameof(healthInformation));
        }

        if (height <= 0)
        {
            return Errors.General.ValueIsInvalid(nameof(height));
        }

        if (weight <= 0)
        {
            return Errors.General.ValueIsInvalid(nameof(weight));
        }

        return new MedicalInformation(healthInformation, height, weight, isVaccinated, isCastrated);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return IsCastrated;
        yield return IsVaccinated;
        yield return HealthInformation;
        yield return Height;
        yield return Weight;
    }
}
