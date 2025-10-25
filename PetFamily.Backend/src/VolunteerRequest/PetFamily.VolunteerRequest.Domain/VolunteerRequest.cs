using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.VolunteerRequest.Domain.Events;

namespace PetFamily.VolunteerRequest.Domain;

public class VolunteerRequest : AggregateRoot<VolunteerRequestId>
{
    public Guid? AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public VolunteerRequestStatus Status { get; private set; }
    public string RejectionComment { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? RejectedAt { get; private set; }

    //ef core
    private VolunteerRequest(VolunteerRequestId id) : base(id) { }

    private VolunteerRequest(
        VolunteerRequestId id,
        Guid userId,
        VolunteerInfo volunteerInfo) : base(id)
    {
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

        VolunteerRequestTakenForReviewDomainEvent @event = new(
            Id,
            UserId,
            adminId);
        AddDomainEvent(@event);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Submit(Guid userId)
    {
        if (userId != UserId)
            return Errors.VolunteerRequest.InvalidUser();

        if (Status != VolunteerRequestStatus.Created
            && Status != VolunteerRequestStatus.RevisionRequired)
            return Error.Validation("request.invalidStatus", "Request can only be submitted from 'Created' status", nameof(Status));

        Status = VolunteerRequestStatus.Submitted;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Reject(Guid adminId, string rejectionComment)
    {
        if (adminId != AdminId)
            return Errors.VolunteerRequest.InvalidAdmin();

        if (Status != VolunteerRequestStatus.OnReview)
            return Error.Validation("request.invalidStatus", "Request can only be rejected from 'OnReview' status", nameof(Status));

        if (string.IsNullOrWhiteSpace(rejectionComment))
            return Error.Validation("reject.comment.required", "Rejection comment is required", nameof(rejectionComment));

        Status = VolunteerRequestStatus.Rejected;
        RejectionComment = rejectionComment.Trim();
        RejectedAt = DateTime.UtcNow;

        return UnitResult.Success<Error>();
    }


    public UnitResult<Error> Approve(Guid adminId)
    {
        if (adminId != AdminId)
            return Errors.VolunteerRequest.InvalidAdmin();

        if (Status != VolunteerRequestStatus.OnReview)
            return Error.Validation("request.invalidStatus", "Request can only be approved from 'OnReview' status", nameof(Status));

        Status = VolunteerRequestStatus.Approved;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RequestRevision(Guid adminId, string rejectionComment)
    {
        if (adminId != AdminId)
            return Errors.VolunteerRequest.InvalidAdmin();

        if (Status != VolunteerRequestStatus.OnReview)
            return Error.Validation("request.invalidStatus", "Request can only be requested to revision from 'OnReview' status", nameof(Status));

        if (string.IsNullOrWhiteSpace(rejectionComment))
            return Error.Validation("revision.comment.required", "Revision comment is required", nameof(rejectionComment));

        Status = VolunteerRequestStatus.RevisionRequired;
        RejectionComment = rejectionComment.Trim();

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateVolunteerInfo(Guid userId, VolunteerInfo newVolunteerInfo)
    {
        if (userId != UserId)
            return Errors.VolunteerRequest.InvalidUser();

        if (Status != VolunteerRequestStatus.Created
            && Status != VolunteerRequestStatus.RevisionRequired)
            return Error.Validation("request.invalidStatus", "Request can only be submitted from 'Created' status", nameof(Status));


        VolunteerInfo = newVolunteerInfo;

        return UnitResult.Success<Error>();
    }
}