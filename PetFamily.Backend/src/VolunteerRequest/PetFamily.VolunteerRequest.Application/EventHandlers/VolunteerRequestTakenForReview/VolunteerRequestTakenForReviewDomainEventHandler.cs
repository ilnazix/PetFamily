using MediatR;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Contracts.Messaging;
using PetFamily.VolunteerRequest.Domain.Events;

namespace PetFamily.VolunteerRequest.Application.EventHandlers.VolunteerRequestTakenForReview;

internal class VolunteerRequestTakenForReviewDomainEventHandler 
    : INotificationHandler<VolunteerRequestTakenForReviewDomainEvent>
{
    private readonly IOutboxRepository _outboxRepository;

    public VolunteerRequestTakenForReviewDomainEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
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

        await _outboxRepository.Add(integrationEvent, cancellationToken);
    }
}
