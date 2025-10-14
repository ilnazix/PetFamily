using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.RequireRevision;

public record RequireRevisionCommand(
    Guid VolunteerRequestId,
    Guid AdminId,
    string RejectionComment
    ) : ICommand;
