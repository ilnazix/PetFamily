using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Application.Commands;

public interface IDiscussionsRepository
{
    Task<Result<Discussion, Error>> GetById(DiscussionId id, CancellationToken cancellationToken = default);
    Task<Guid> Add(Discussion discussion, CancellationToken cancellationToken = default);
}
