using CSharpFunctionalExtensions;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Contracts;

public interface IDiscussionsModule
{
    public Task<Result<Guid, ErrorList>> CreateDiscussion(CreateDiscussionRequest request, CancellationToken cancellationToken);
}
