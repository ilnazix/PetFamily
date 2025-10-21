namespace PetFamily.Core.Database;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}