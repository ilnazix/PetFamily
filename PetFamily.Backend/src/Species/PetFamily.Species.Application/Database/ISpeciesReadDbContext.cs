using PetFamily.Species.Application.DTOs;
using PetFamily.Species.Domain.Models;

namespace PetFamily.Species.Application.Database
{
    public interface ISpeciesReadDbContext
    {
        IQueryable<SpeciesDto> Species { get; } 
        IQueryable<BreedDto> Breeds{ get; } 
    }
}
