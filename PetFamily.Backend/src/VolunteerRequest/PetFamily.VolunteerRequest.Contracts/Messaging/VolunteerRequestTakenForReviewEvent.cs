namespace PetFamily.VolunteerRequest.Contracts.Messaging;

public record VolunteerRequestTakenForReviewEvent(
    Guid VolunteerRequestId,
    Guid UserId,
    Guid AdminId);
