using Microsoft.EntityFrameworkCore;
using PetFamily.VolunteerRequest.Application.Database;
using PetFamily.VolunteerRequest.Application.DTOs;

namespace PetFamily.VolunteerRequest.Infrastructure.DbContexts;

internal class VolunteerRequestsReadDbContext : DbContext, IVolunteerRequestsReadDbContext
{
    IQueryable<VolunteerRequestDto> IVolunteerRequestsReadDbContext.VolunteerRequests => Set<VolunteerRequestDto>();

    public VolunteerRequestsReadDbContext(
        DbContextOptions<VolunteerRequestsReadDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.SCHEMA);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerRequestsReadDbContext).Assembly,
            type => type.FullName?.Contains(Constants.READ_DB_CONTEXT_CONFIGURATIONS) ?? false);
    }
}
