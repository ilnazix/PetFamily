using MediatR;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteerRequest.Domain.Events;

namespace PetFamily.VolunteerRequest.Application.EventHandlers.VolunteerRequestTakenForReview;

internal class VolunteerRequestTakenForReviewDomainEventHandler 
    : INotificationHandler<VolunteerRequestTakenForReviewDomainEvent>
{
    private readonly ILogger<VolunteerRequestTakenForReviewDomainEventHandler> _logger;

    public VolunteerRequestTakenForReviewDomainEventHandler(
        ILogger<VolunteerRequestTakenForReviewDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        VolunteerRequestTakenForReviewDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DOMAIN EVENT: Volunteer request with id: {VolunteerRequestId} taken for review by admin with id: {AdminId} (11111)",
            notification.volunteerId,
            notification.AdminId);

        return Task.CompletedTask;
    }
}

internal class VolunteerRequestTakenForReviewDomainEventHandlerSecond
    : INotificationHandler<VolunteerRequestTakenForReviewDomainEvent>
{
    private readonly ILogger<VolunteerRequestTakenForReviewDomainEventHandler> _logger;

    public VolunteerRequestTakenForReviewDomainEventHandlerSecond(
        ILogger<VolunteerRequestTakenForReviewDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        VolunteerRequestTakenForReviewDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DOMAIN EVENT: Volunteer request with id: {VolunteerRequestId} taken for review by admin with id: {AdminId} (22222)",
            notification.volunteerId,
            notification.AdminId);

        return Task.CompletedTask;
    }
}
