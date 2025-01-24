using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers
{
    public class Position : ValueObject
    {
        public int Value { get; }

        private Position(int value)
        {
            Value = value;
        }

        public static Result<Position, Error> Create(int position)
        {
            if(position < 1)
            {
                return Errors.General.ValueIsInvalid(nameof(position));
            }

            return new Position(position);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
