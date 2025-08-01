using Microsoft.EntityFrameworkCore;
using PetFamily.Species.Domain.Models;

namespace PetFamily.Species.Infrastructure.DbContexts
{
    public class SpeciesWriteDbContext : DbContext
    {
        public DbSet<Domain.Models.Species> Species { get; set; }
        public DbSet<Breed> Breeds { get; set; }

        public SpeciesWriteDbContext(DbContextOptions<SpeciesWriteDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains(Constants.WRITE_DB_CONTEXT_CONFIGURATIONS) ?? false);

            modelBuilder.HasDefaultSchema(Constants.SCHEMA);
        }
    }
}
