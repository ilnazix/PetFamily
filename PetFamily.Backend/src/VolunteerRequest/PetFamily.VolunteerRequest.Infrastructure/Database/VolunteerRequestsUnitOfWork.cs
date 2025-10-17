using PetFamily.VolunteerRequest.Application.Commands;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;

namespace PetFamily.VolunteerRequest.Infrastructure.Database;

internal class VolunteerRequestsUnitOfWork : IVolunteerRequestUnitOfWork
{
    private readonly VolunteerRequestsWriteDbContext _dbContext;

    public IVolunteerRequestsRepository VolunteerRequestsRepository { get; private set; }

    public VolunteerRequestsUnitOfWork(
        VolunteerRequestsWriteDbContext dbContext,
        IVolunteerRequestsRepository volunteerRequestsRepository)
    {
        _dbContext = dbContext;
        VolunteerRequestsRepository = volunteerRequestsRepository;
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
