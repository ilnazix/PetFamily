using PetFamily.Core.Database;

namespace PetFamily.Discussions.Application.Commands;

public interface IDiscussionsUnitOfWork : IUnitOfWork
{
    IDiscussionsRepository DiscussionsRepository { get; }
}