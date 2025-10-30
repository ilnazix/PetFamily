using CSharpFunctionalExtensions;


namespace PetFamily.SharedKernel.ValueObjects;

public class Position : ComparableValueObject
{
    public int Value { get; }

    private Position(int value)
    {
        Value = value;
    }

    public Result<Position, Error> MoveForward() => Create(Value + 1);

    public Result<Position, Error> MoveBack() => Create(Value - 1);

    public static Result<Position, Error> Create(int position)
    {
        if (position < 1)
        {
            return Errors.General.ValueIsInvalid(nameof(position));
        }

        return new Position(position);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}
