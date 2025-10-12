using CSharpFunctionalExtensions;
using PetFamily.Discussions.Application.Commands.CreateDiscussion;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.Discussions.Presentation.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Presentation;

public class DiscussionsModule : IDiscussionsModule
{
    private readonly CreateDiscussionCommandHandler _createDiscussionCommandHandler;

    public DiscussionsModule(CreateDiscussionCommandHandler createDiscussionCommandHandler)
    {
        _createDiscussionCommandHandler = createDiscussionCommandHandler;
    }

    public Task<Result<Guid, ErrorList>> CreateDiscussion(
        CreateDiscussionRequest request, 
        CancellationToken cancellationToken)
    {
        return _createDiscussionCommandHandler.Handle(request.ToCommand(), cancellationToken);
    }
}
