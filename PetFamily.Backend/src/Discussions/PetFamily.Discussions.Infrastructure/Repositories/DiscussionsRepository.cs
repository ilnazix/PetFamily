using PetFamily.Discussions.Application.Commands;
using PetFamily.Discussions.Domain;
using PetFamily.Discussions.Infrastructure.DbContexts;

namespace PetFamily.Discussions.Infrastructure.Repositories;

internal class DiscussionsRepository : IDiscussionsRepository
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public DiscussionsRepository(DiscussionsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Discussion discussion, CancellationToken cancellationToken = default)
    {
        await  _dbContext.AddAsync(discussion, cancellationToken);

        return discussion.Id;
    }
}
