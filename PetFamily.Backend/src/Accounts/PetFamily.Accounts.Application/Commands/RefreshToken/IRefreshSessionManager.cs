using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RefreshToken;

public interface IRefreshSessionManager
{
    void Delete(RefreshSession session);
    Task<Result<RefreshSession, Error>> GetByRefreshToken(string refreshToken, CancellationToken cancellationToken);
    Task Add(RefreshSession session, CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
}
