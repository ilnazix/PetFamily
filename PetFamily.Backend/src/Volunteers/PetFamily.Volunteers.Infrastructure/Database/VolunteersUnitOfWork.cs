using PetFamily.Volunteers.Application.Volunteers.Commands;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure.Database;

internal class VolunteersUnitOfWork : IVolunteersUnitOfWork
{
    private readonly VolunteersWriteDbContext _dbContext;

    public VolunteersUnitOfWork(
        IVolunteersRepository volunteersRepository,
        VolunteersWriteDbContext dbContext)
    {
        VolunteersRepository = volunteersRepository;
        _dbContext = dbContext;
    }
    public IVolunteersRepository VolunteersRepository { get; }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
