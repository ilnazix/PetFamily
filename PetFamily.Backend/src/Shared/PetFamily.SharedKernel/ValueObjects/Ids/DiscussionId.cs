using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects.Ids;

public class DiscussionId : ComparableValueObject
{
    private DiscussionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static DiscussionId NewDiscussionId() => new DiscussionId(Guid.NewGuid());
    public static DiscussionId Empty() => new DiscussionId(Guid.Empty);
    public static DiscussionId Create(Guid id) => new DiscussionId(id);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(DiscussionId id) => id.Value;
}