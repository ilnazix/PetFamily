using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequest.Domain;

public class VolunteerRequestStatus : ComparableValueObject
{
    public static readonly VolunteerRequestStatus Created = new(nameof(Created).ToLower());
    public static readonly VolunteerRequestStatus Submitted = new(nameof(Submitted).ToLower());
    public static readonly VolunteerRequestStatus Rejected = new(nameof(Rejected).ToLower());
    public static readonly VolunteerRequestStatus OnReview = new(nameof(OnReview).ToLower());
    public static readonly VolunteerRequestStatus Approved = new(nameof(Approved).ToLower());
    public static readonly VolunteerRequestStatus RevisionRequired = new(nameof(RevisionRequired).ToLower());

    private static readonly VolunteerRequestStatus[] _all =
    [
        Created,
        Submitted,
        Rejected,
        OnReview,
        Approved,
        RevisionRequired
    ];

    public string Value { get; }

    private VolunteerRequestStatus(string value)
    {
        Value = value;
    }

    public static Result<VolunteerRequestStatus, Error> Create(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return Errors.General.ValueIsRequired(nameof(status));
        }

        status = status.Trim().ToLower();

        if (_all.Any(s => s.Value == status) == false)
        {
            return Errors.General.ValueIsInvalid(nameof(status));
        }

        return new VolunteerRequestStatus(status);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}


