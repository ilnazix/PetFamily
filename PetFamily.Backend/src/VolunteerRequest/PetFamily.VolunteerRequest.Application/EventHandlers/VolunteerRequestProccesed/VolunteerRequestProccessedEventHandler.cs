using MediatR;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Contracts.Messaging;
using PetFamily.VolunteerRequest.Domain.Events;

namespace PetFamily.VolunteerRequest.Application.EventHandlers.VolunteerRequestProccesed;

internal class VolunteerRequestProccessedEventHandler : INotificationHandler<VolunteerRequestProcessedDomainEvent>
{
    private readonly IOutboxRepository _outboxRepository;

    public VolunteerRequestProccessedEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(
        VolunteerRequestProcessedDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        var @event = new VolunteerRequestProcessedEvent(notification.Id.Value);

        await _outboxRepository.Add(@event, cancellationToken);
    }
}
