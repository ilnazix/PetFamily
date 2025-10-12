using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands;

public interface IDiscussionsRepository
{
    Task<Guid> Add(Discussion discussion, CancellationToken cancellationToken = default);
}
