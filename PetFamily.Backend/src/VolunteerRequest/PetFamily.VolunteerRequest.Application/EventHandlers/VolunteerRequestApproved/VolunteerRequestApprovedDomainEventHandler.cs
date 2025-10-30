using MediatR;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Contracts.Messaging;
using PetFamily.VolunteerRequest.Domain.Events;

namespace PetFamily.VolunteerRequest.Application.EventHandlers.VolunteerRequestApproved;

internal class VolunteerRequestApprovedDomainEventHandler 
    : INotificationHandler<VolunteerRequestApprovedDomainEvent>
{
    private readonly IOutboxRepository _outboxRepository;

    public VolunteerRequestApprovedDomainEventHandler(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(
        VolunteerRequestApprovedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var message = new VolunteerRequestApprovedEvent
        (
            VolunteerRequestId: notification.VolunteerRequestId.Value,
            UserId: notification.UserId,
            FirstName: notification.VolunteerInfo.FullName.FirstName,
            LastName: notification.VolunteerInfo.FullName.LastName,
            MiddleName: notification.VolunteerInfo.FullName.MiddleName,
            PhoneNumber: notification.VolunteerInfo.PhoneNumber.Value,
            Email: notification.VolunteerInfo.Email.Value
        );

        await _outboxRepository.Add(message, cancellationToken);
    }
}
