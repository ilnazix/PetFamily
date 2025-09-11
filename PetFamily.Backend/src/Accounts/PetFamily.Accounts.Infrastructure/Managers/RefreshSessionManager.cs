using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Managers;

internal class RefreshSessionManager 
{
    private readonly AccountsDbContext _dbContext;

    public RefreshSessionManager(AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Save(RefreshSession session, CancellationToken cancellationToken)
    {
        _dbContext.Add(session);
        return _dbContext.SaveChangesAsync();
    }
}
