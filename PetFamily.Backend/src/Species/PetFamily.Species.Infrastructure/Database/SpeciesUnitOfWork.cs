using PetFamily.Species.Application.Species.Commands;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure.Database;

internal class SpeciesUnitOfWork : ISpeciesUnitOfWork
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly SpeciesWriteDbContext _dbContext;

    public SpeciesUnitOfWork(
        ISpeciesRepository speciesRepository,
        SpeciesWriteDbContext dbContext)
    {
        _speciesRepository = speciesRepository;
        _dbContext = dbContext;
    }

    public ISpeciesRepository SpeciesRepository => _speciesRepository;

    public Task Commit(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
