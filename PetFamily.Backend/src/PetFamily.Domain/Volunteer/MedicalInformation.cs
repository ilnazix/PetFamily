using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer
{
    public record MedicalInformation
    {
        bool IsCastrated { get; }
        bool IsVaccinated { get; }
        string HealthInformation { get; }
        int Height { get; }
        int Weight { get; }

        private MedicalInformation(string healthInformation, int height, int weight, bool isVaccinated, bool isCastrated)
        {
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            HealthInformation = healthInformation;
            Height = height;
            Weight = weight;
        }

        public static Result<MedicalInformation> Create(string healthInformation, int height, int weight, bool isVaccinated, bool isCastrated)
        {
            string errors = string.Empty;

            if (string.IsNullOrWhiteSpace(healthInformation))
            {
                errors += "Health information cannot be empty\n";
            }

            if (height <= 0)
            {
                errors += "Height cannot be less or equal zero\n";
            }

            if (weight <= 0)
            {
                errors += "Weight cannot be less or equal zero\n";
            }

            if (string.IsNullOrEmpty(errors))
            { 
                return Result.Success<MedicalInformation>(new MedicalInformation(healthInformation, height, weight, isVaccinated, isCastrated));
            }

            return Result.Failure<MedicalInformation>(errors);
        }
    }
}
