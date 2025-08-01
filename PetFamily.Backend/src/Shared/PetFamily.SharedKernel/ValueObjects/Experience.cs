using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects
{
    public class Experience : ValueObject
    {
        public const int MIN_VALUE = 0;
        public const int MAX_VALUE = 80;

        public int Value { get; }

        private Experience(int value)
        {
            Value = value;
        }

        public static Result<Experience, Error> Create(int experience)
        {
            if (experience < MIN_VALUE || experience > MAX_VALUE)
            {
                return Errors.General.ValueIsInvalid(nameof(experience));
            }

            return new Experience(experience);
        }

        public static Experience Default() => new Experience(0);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
