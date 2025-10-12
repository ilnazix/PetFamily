using PetFamily.Discussions.Application.Commands;
using PetFamily.Discussions.Infrastructure.DbContexts;

namespace PetFamily.Discussions.Infrastructure.Database;

internal class DiscussionsUnitOfWork : IDiscussionsUnitOfWork
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public IDiscussionsRepository DiscussionsRepository { get; private set; }

    public DiscussionsUnitOfWork(
        DiscussionsWriteDbContext dbContext,
        IDiscussionsRepository discussionsRepository)
    {
        _dbContext = dbContext;
        DiscussionsRepository = discussionsRepository;
    }


    public Task Commit(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
