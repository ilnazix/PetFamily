using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel;

public abstract class AggregateRoot<Tid> : Entity<Tid>
    where Tid : IComparable<Tid> 
{
    private readonly List<IDomainEvent> _domainEvents = new();
    
    protected AggregateRoot(Tid id) 
        : base(id) {}

    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    internal void ClearDomainEvents() => _domainEvents.Clear();
}
