using PetFamily.Core.Database;

namespace PetFamily.VolunteerRequest.Application.Commands;

public interface IVolunteerRequestUnitOfWork : IUnitOfWork
{
    IVolunteerRequestsRepository VolunteerRequestsRepository { get; }
}