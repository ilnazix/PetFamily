using Microsoft.EntityFrameworkCore;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Application.DTOs;

namespace PetFamily.Species.Infrastructure.DbContexts;

public class SpeciesReadDbContext : DbContext, ISpeciesReadDbContext
{
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

    public IQueryable<BreedDto> Breeds => Set<BreedDto>();

    public SpeciesReadDbContext(DbContextOptions<SpeciesReadDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SpeciesReadDbContext).Assembly,
            type => type.FullName?.Contains(Constants.READ_DB_CONTEXT_CONFIGURATIONS) ?? false);

        modelBuilder.HasDefaultSchema(Constants.SCHEMA);
    }
}
