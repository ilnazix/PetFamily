using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Domain.Events;

public record VolunteerRequestTakenForReviewDomainEvent(
    VolunteerRequestId VolunteerRequestId,
    Guid UserId,
    Guid AdminId) : IDomainEvent;
