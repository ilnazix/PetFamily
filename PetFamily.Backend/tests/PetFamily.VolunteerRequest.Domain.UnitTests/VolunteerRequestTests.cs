using FluentAssertions;
using PetFamily.VolunteerRequest.Domain.Events;
using PetFamily.VolunteerRequest.Domain.UnitTests.Builders;

namespace PetFamily.VolunteerRequest.Domain.UnitTests;


public class VolunteerRequestTests
{
    private readonly VolunteerRequestBuilder _builder;
    public VolunteerRequestTests()
    {
        _builder = new VolunteerRequestBuilder();
    }

    [Fact]
    public void TakeOnReview_StatusSubmitted_ChangeStatusToOnReview()
    {
        //Arrange
        var volunteerRequestWithSubmittedStatus = _builder.CreateWithSubmittedStatus();
        var adminId = Guid.NewGuid();

        //Act
        var result = volunteerRequestWithSubmittedStatus.TakeOnReview(adminId);

        //Assert
        var newStatus = volunteerRequestWithSubmittedStatus.Status;
        var events = volunteerRequestWithSubmittedStatus.DomainEvents;

        result.IsSuccess.Should().BeTrue();
        events.Should().ContainSingle(e => e is VolunteerRequestTakenForReviewDomainEvent);
        newStatus.Should().Be(VolunteerRequestStatus.OnReview);
    }

    [Fact]
    public void Submit_StatusCreated_ChangeStatusToSubmitted()
    {
        //Arrange
        var volunteerRequest = _builder.CreateDefault();
        var userId = volunteerRequest.UserId;

        //Act
        var result = volunteerRequest.Submit(userId);

        //Assert
        var newStatus = volunteerRequest.Status;

        result.IsSuccess.Should().BeTrue();
        newStatus.Should().Be(VolunteerRequestStatus.Submitted);
    }

    [Fact]
    public void Reject_StatusOnReview_ChangeStatusToRejected()
    {
        //Arrange
        var volunteerRequestWithOnReviewStatus = _builder.CreateWithOnReviewStatus(Guid.NewGuid());
        var rejectionComment = "some rejection comment";
        var adminId = volunteerRequestWithOnReviewStatus.AdminId!.Value;

        //Act
        var result = volunteerRequestWithOnReviewStatus.Reject(adminId, rejectionComment);

        //Assert
        var newStatus = volunteerRequestWithOnReviewStatus.Status;

        result.IsSuccess.Should().BeTrue();
        newStatus.Should().Be(VolunteerRequestStatus.Rejected);
    }

    [Fact]
    public void Approve_StatusOnReview_ChangeStatusToApproved()
    {
        //Arrange
        var volunteerRequestWithOnReviewStatus = _builder.CreateWithOnReviewStatus(Guid.NewGuid());
        var adminId = volunteerRequestWithOnReviewStatus.AdminId!.Value;

        //Act
        var result = volunteerRequestWithOnReviewStatus.Approve(adminId);

        //Assert
        var newStatus = volunteerRequestWithOnReviewStatus.Status;

        result.IsSuccess.Should().BeTrue();
        newStatus.Should().Be(VolunteerRequestStatus.Approved);
    }

    [Fact]
    public void RequestRevision_StatusOnReview_ChangeStatusToRevisionRequired()
    {
        //Arrange
        var volunteerRequestWithOnReviewStatus = _builder.CreateWithOnReviewStatus(Guid.NewGuid());
        var rejectionComment = "some rejection comment";
        var adminId = volunteerRequestWithOnReviewStatus.AdminId!.Value;

        //Act
        var result = volunteerRequestWithOnReviewStatus.RequestRevision(adminId, rejectionComment);

        //Assert
        var newStatus = volunteerRequestWithOnReviewStatus.Status;

        result.IsSuccess.Should().BeTrue();
        newStatus.Should().Be(VolunteerRequestStatus.RevisionRequired);
    }

    [Theory]
    [InlineData(nameof(VolunteerRequestStatus.Created))]
    [InlineData(nameof(VolunteerRequestStatus.Approved))]
    [InlineData(nameof(VolunteerRequestStatus.Rejected))]
    [InlineData(nameof(VolunteerRequestStatus.RevisionRequired))]
    public void TakeOnReview_InvalidStatuses_ReturnsError(string statusName)
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var status = VolunteerRequestStatus.Create(statusName).Value;
        var request = new VolunteerRequestBuilder().CreateWithStatus(status);
        var oldStatus = request.Status;

        // Act
        var result = request.TakeOnReview(adminId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        request.Status.Should().Be(oldStatus);
    }

    [Theory]
    [InlineData(nameof(VolunteerRequestStatus.OnReview))]
    [InlineData(nameof(VolunteerRequestStatus.Approved))]
    [InlineData(nameof(VolunteerRequestStatus.Rejected))]
    public void Submit_InvalidStatuses_ReturnsError(string statusName)
    {
        // Arrange
        var status = VolunteerRequestStatus.Create(statusName).Value;
        var request = new VolunteerRequestBuilder().CreateWithStatus(status);
        var oldStatus = request.Status;
        var adminId = request.AdminId!.Value;

        // Act
        var result = request.Submit(adminId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        request.Status.Should().Be(oldStatus);
    }

    [Theory]
    [InlineData(nameof(VolunteerRequestStatus.Created))]
    [InlineData(nameof(VolunteerRequestStatus.Submitted))]
    [InlineData(nameof(VolunteerRequestStatus.Approved))]
    [InlineData(nameof(VolunteerRequestStatus.Rejected))]
    [InlineData(nameof(VolunteerRequestStatus.RevisionRequired))]
    public void Reject_InvalidStatuses_ReturnsError(string statusName)
    {
        // Arrange
        var status = VolunteerRequestStatus.Create(statusName).Value;
        var request = new VolunteerRequestBuilder().CreateWithStatus(status);
        var oldStatus = request.Status;
        var adminId = request.AdminId ?? Guid.Empty;

        // Act
        var result = request.Reject(adminId, "Some comment");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        request.Status.Should().Be(oldStatus);
    }

    [Theory]
    [InlineData(nameof(VolunteerRequestStatus.Created))]
    [InlineData(nameof(VolunteerRequestStatus.Submitted))]
    [InlineData(nameof(VolunteerRequestStatus.Approved))]
    [InlineData(nameof(VolunteerRequestStatus.Rejected))]
    [InlineData(nameof(VolunteerRequestStatus.RevisionRequired))]
    public void Approve_InvalidStatuses_ReturnsError(string statusName)
    {
        // Arrange
        var status = VolunteerRequestStatus.Create(statusName).Value;
        var request = new VolunteerRequestBuilder().CreateWithStatus(status);
        var oldStatus = request.Status;
        var adminId = request.AdminId ?? Guid.Empty;

        // Act
        var result = request.Approve(adminId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        request.Status.Should().Be(oldStatus);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void RequestRevision_EmptyOrNullComment_ReturnsError(string invalidComment)
    {
        // Arrange
        var request = new VolunteerRequestBuilder()
            .CreateWithOnReviewStatus(Guid.NewGuid());
        var oldStatus = request.Status;
        var adminId = request.AdminId!.Value;

        // Act
        var result = request.RequestRevision(adminId, invalidComment);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        request.Status.Should().Be(oldStatus);
    }

    [Theory]
    [InlineData(nameof(VolunteerRequestStatus.Created))]
    [InlineData(nameof(VolunteerRequestStatus.Submitted))]
    [InlineData(nameof(VolunteerRequestStatus.Approved))]
    [InlineData(nameof(VolunteerRequestStatus.Rejected))]
    [InlineData(nameof(VolunteerRequestStatus.RevisionRequired))]
    public void RequestRevision_StatusNotOnReview_ReturnsError(string statusName)
    {
        // Arrange
        var status = VolunteerRequestStatus.Create(statusName).Value;
        var request = new VolunteerRequestBuilder().CreateWithStatus(status);
        var oldStatus = request.Status;
        var adminId = request.AdminId ?? Guid.Empty;

        // Act
        var result = request.RequestRevision(adminId, "Some comment");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        request.Status.Should().Be(oldStatus);
    }

}
