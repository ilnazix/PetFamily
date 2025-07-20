using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;

namespace PetFamily.Infrastructure.DbContexts
{
    public class ApplicationReadDbContext : DbContext, IReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();

        public IQueryable<PetDto> Pets => Set<PetDto>();

        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

        public IQueryable<BreedDto> Breeds => Set<BreedDto>(); 

        public ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationReadDbContext).Assembly, 
                type => type.FullName?.Contains(Constants.READ_DB_CONTEXT_CONFIGURATIONS) ?? false);
        }
    }
}
