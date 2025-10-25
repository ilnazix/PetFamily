using MediatR;

namespace PetFamily.SharedKernel;

public static class MediatrExtesions
{
    public static async Task PublishDomainEvents<TId>(
        this IPublisher publisher,
        AggregateRoot<TId> aggregateRoot,
        CancellationToken cancellationToken = default) where TId : IComparable<TId>
    {
        foreach (var domainEvent in aggregateRoot.DomainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        aggregateRoot.ClearDomainEvents();
    }
}
