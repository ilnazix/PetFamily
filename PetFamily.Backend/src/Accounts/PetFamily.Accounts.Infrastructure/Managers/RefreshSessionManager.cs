using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Commands.RefreshToken;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Managers;

internal class RefreshSessionManager : IRefreshSessionManager
{
    private readonly AccountsWriteDbContext _dbContext;

    public RefreshSessionManager(AccountsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(
        RefreshSession session, 
        CancellationToken cancellationToken)
    {
        await _dbContext.RefreshSessions.AddAsync(session, cancellationToken);
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var session = await _dbContext.RefreshSessions
            .Include(s => s.User)
            .AsNoTracking()
            .SingleOrDefaultAsync(
                rt => rt.RefreshToken == Guid.Parse(refreshToken), 
                cancellationToken);

        if (session is null) return Errors.User.TokenExpired();

        return session;
    }

    public void Delete(RefreshSession session)
    {
        _dbContext.RefreshSessions
            .Remove(session);
    }

    public Task Save(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
