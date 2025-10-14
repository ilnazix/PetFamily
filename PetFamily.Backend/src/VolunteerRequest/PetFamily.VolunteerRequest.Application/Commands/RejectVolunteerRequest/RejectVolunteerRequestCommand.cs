using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.RejectVolunteerRequest;

public record RejectVolunteerRequestCommand(
    Guid VolunteerRequestId,
    Guid AdminId,
    string RejectionComment
    ) : ICommand;