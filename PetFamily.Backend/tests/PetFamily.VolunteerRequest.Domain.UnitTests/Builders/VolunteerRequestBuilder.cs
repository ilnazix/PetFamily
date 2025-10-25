using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Domain.UnitTests.Builders;

public class VolunteerRequestBuilder
{
    private Guid _userId = Guid.NewGuid();
    private VolunteerInfo _volunteerInfo = DefaultVolunteerInfo();

    public VolunteerRequestBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public VolunteerRequestBuilder WithVolunteerInfo(VolunteerInfo info)
    {
        _volunteerInfo = info;
        return this;
    }

    public VolunteerRequest CreateDefault()
    {
        var id = VolunteerRequestId.NewVolunteerId();
        return VolunteerRequest.Create(id, _userId, _volunteerInfo).Value;
    }

    public VolunteerRequest CreateWithSubmittedStatus()
    {
        var request = CreateDefault();
        request.Submit(request.UserId);
        return request;
    }

    public VolunteerRequest CreateWithOnReviewStatus(Guid adminId)
    {
        var request = CreateWithSubmittedStatus();
        request.TakeOnReview(adminId);
        return request;
    }

    public VolunteerRequest CreateWithApprovedStatus(Guid adminId)
    {
        var request = CreateWithOnReviewStatus(adminId);
        request.Approve(adminId);
        return request;
    }

    public VolunteerRequest CreateWithRejectedStatus(Guid adminId, string rejectionComment)
    {
        var request = CreateWithOnReviewStatus(adminId);
        request.Reject(adminId, rejectionComment);
        return request;
    }

    public VolunteerRequest CreateWithRevisionRequiredStatus(Guid adminId, string rejectionComment)
    {
        var request = CreateWithOnReviewStatus(adminId);
        request.RequestRevision(adminId,rejectionComment);
        return request;
    }

    public VolunteerRequest CreateWithStatus(
    VolunteerRequestStatus status,
    Guid? adminId = null,
    string? comment = null)
    {
        if (status == VolunteerRequestStatus.Created)
            return CreateDefault();

        if (status == VolunteerRequestStatus.Submitted)
            return CreateWithSubmittedStatus();

        if (status == VolunteerRequestStatus.OnReview)
            return CreateWithOnReviewStatus(adminId ?? Guid.NewGuid());

        if (status == VolunteerRequestStatus.Approved)
            return CreateWithApprovedStatus(adminId ?? Guid.NewGuid());

        if (status == VolunteerRequestStatus.Rejected)
            return CreateWithRejectedStatus(adminId ?? Guid.NewGuid(), comment ?? "Rejected for test purposes");

        if (status == VolunteerRequestStatus.RevisionRequired)
            return CreateWithRevisionRequiredStatus(adminId ?? Guid.NewGuid(), comment ?? "Revision required for test purposes");

        throw new ArgumentOutOfRangeException(nameof(status), status, "Unsupported status");
    }

    private static VolunteerInfo DefaultVolunteerInfo()
    {
        var fullName = FullName.Create("Иван", "Петров", "Алексеевич").Value;
        var phone = PhoneNumber.Create("+79998887766").Value;
        var email = Email.Create("ivan.petrov@example.com").Value;

        return VolunteerInfo.Create(fullName, phone, email).Value;
    }
}