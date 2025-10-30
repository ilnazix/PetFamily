using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Domain.Events;

public record VolunteerRequestProcessedDomainEvent(VolunteerRequestId Id) : IDomainEvent;
