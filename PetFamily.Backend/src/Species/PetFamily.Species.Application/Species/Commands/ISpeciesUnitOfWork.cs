using PetFamily.Core.Database;

namespace PetFamily.Species.Application.Species.Commands;
public interface ISpeciesUnitOfWork : IUnitOfWork
{
    ISpeciesRepository SpeciesRepository { get; }
}
