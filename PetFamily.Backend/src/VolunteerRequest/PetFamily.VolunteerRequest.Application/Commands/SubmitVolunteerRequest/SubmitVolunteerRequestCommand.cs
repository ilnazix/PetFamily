using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.SubmitVolunteerRequest;

public record SubmitVolunteerRequestCommand(Guid VolunteerRequestId, Guid UserId) : ICommand;