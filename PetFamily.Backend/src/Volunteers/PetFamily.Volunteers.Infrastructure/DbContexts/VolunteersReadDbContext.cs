using Microsoft.EntityFrameworkCore;
using PetFamily.Volunteers.Application.DTOs;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Infrastructure.DbContexts
{
    public class VolunteersReadDbContext : DbContext, IVolunteersReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();

        public IQueryable<PetDto> Pets => Set<PetDto>();


        public VolunteersReadDbContext(DbContextOptions<VolunteersReadDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteersReadDbContext).Assembly,
                type => type.FullName?.Contains(Constants.READ_DB_CONTEXT_CONFIGURATIONS) ?? false);

            modelBuilder.HasDefaultSchema(Constants.SCHEMA);
        }
    }
}
