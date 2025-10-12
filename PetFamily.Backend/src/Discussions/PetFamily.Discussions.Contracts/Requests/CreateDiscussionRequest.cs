using PetFamily.Discussions.Contracts.Models;

namespace PetFamily.Discussions.Contracts.Requests;

public record CreateDiscussionRequest(
    Guid RelationId, 
    IEnumerable<DiscussionParticipant> Participants);

