using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Domain.Events;

public record VolunteerRequestTakenForReviewDomainEvent(
    VolunteerRequestId volunteerId,
    Guid UserId,
    Guid AdminId) : IDomainEvent;
