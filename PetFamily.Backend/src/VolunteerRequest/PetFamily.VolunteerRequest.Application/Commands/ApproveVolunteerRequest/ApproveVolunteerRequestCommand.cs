using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.VolunteerRequest.Application.Commands.ApproveVolunteerRequest;

public record ApproveVolunteerRequestCommand(
    Guid VolunteerRequestId,
    Guid AdminId) : ICommand;
