using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Domain;

public class VolunteerRequest : Entity<VolunteerRequestId>
{
    public Guid? AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public VolunteerRequestStatus Status { get; private set; }
    public string RejectionComment { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    //ef core
    private VolunteerRequest() { }

    private VolunteerRequest(
        VolunteerRequestId id,
        Guid userId,
        VolunteerInfo volunteerInfo)
    {
        Id = id;
        UserId = userId;
        VolunteerInfo = volunteerInfo;
        Status = VolunteerRequestStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<VolunteerRequest, Error> Create(
        VolunteerRequestId id,
        Guid userId,
        VolunteerInfo volunteerInfo)
    {
        if (Guid.Empty == userId)
            return Error.Validation("user.id.required", "User id can not be empty", nameof(userId));

        return new VolunteerRequest(id, userId, volunteerInfo);
    }

    public UnitResult<Error> TakeOnReview(Guid adminId)
    {
        if (Status != VolunteerRequestStatus.Submitted)
            return Error.Validation("request.invalidStatus", "Request can only be taken on review from 'Submitted' status", nameof(Status));

        if (Guid.Empty == adminId)
            return Error.Validation("admin.id.required", "Admin id can not be empty", nameof(adminId));

        AdminId = adminId;
        Status = VolunteerRequestStatus.OnReview;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Submit()
    {
        if (Status != VolunteerRequestStatus.Created
            && Status != VolunteerRequestStatus.RevisionRequired)
            return Error.Validation("request.invalidStatus", "Request can only be submitted from 'Created' status", nameof(Status));

        Status = VolunteerRequestStatus.Submitted;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Reject(string rejectionComment)
    {
        if (Status != VolunteerRequestStatus.OnReview)
            return Error.Validation("request.invalidStatus", "Request can only be rejected from 'OnReview' status", nameof(Status));

        if (string.IsNullOrWhiteSpace(rejectionComment))
            return Error.Validation("reject.comment.required", "Rejection comment is required", nameof(rejectionComment));

        Status = VolunteerRequestStatus.Rejected;
        RejectionComment = rejectionComment.Trim();

        return UnitResult.Success<Error>();
    }


    public UnitResult<Error> Approve()
    {
        if (Status != VolunteerRequestStatus.OnReview)
            return Error.Validation("request.invalidStatus", "Request can only be approved from 'OnReview' status", nameof(Status));

        Status = VolunteerRequestStatus.Approved;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RequestRevision(string rejectionComment)
    {
        if (Status != VolunteerRequestStatus.OnReview)
            return Error.Validation("request.invalidStatus", "Request can only be requested to revision from 'OnReview' status", nameof(Status));

        if (string.IsNullOrWhiteSpace(rejectionComment))
            return Error.Validation("revision.comment.required", "Revision comment is required", nameof(rejectionComment));

        Status = VolunteerRequestStatus.RevisionRequired;
        RejectionComment = rejectionComment.Trim();

        return UnitResult.Success<Error>();
    }
}