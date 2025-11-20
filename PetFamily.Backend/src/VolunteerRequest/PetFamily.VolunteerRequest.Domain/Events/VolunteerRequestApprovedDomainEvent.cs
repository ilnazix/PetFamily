using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Domain.Events;

public record VolunteerRequestApprovedDomainEvent(
    VolunteerRequestId VolunteerRequestId,
    Guid UserId,
    VolunteerInfo VolunteerInfo
    ) : IDomainEvent;
