using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Commands.TakeOnReview;

public record TakeRequestOnReviewCommand(
    Guid RequestId,
    Guid AdminId) : ICommand;
