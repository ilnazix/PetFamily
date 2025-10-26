using MassTransit;
using MediatR;
using PetFamily.VolunteerRequest.Contracts.Messaging;
using PetFamily.VolunteerRequest.Domain.Events;

namespace PetFamily.VolunteerRequest.Application.EventHandlers.VolunteerRequestTakenForReview;

internal class VolunteerRequestTakenForReviewDomainEventHandler 
    : INotificationHandler<VolunteerRequestTakenForReviewDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public VolunteerRequestTakenForReviewDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(
        VolunteerRequestTakenForReviewDomainEvent notification, 
        CancellationToken cancellationToken)
    {
       var integrationEvent = new VolunteerRequestTakenForReviewEvent
       (
           VolunteerRequestId: notification.VolunteerRequestId.Value,
           UserId: notification.UserId,
           AdminId: notification.AdminId
       );

        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
