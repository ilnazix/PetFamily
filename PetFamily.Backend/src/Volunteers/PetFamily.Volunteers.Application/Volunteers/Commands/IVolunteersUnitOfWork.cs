using PetFamily.Core.Database;

namespace PetFamily.Volunteers.Application.Volunteers.Commands;

public interface IVolunteersUnitOfWork : IUnitOfWork
{
    IVolunteersRepository VolunteersRepository { get; }
}
