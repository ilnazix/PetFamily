using Microsoft.EntityFrameworkCore;
using PetFamily.VolunteerRequest.Infrastructure.Outbox;


namespace PetFamily.VolunteerRequest.Infrastructure.DbContexts;

internal class VolunteerRequestsWriteDbContext : DbContext
{
    public DbSet<Domain.VolunteerRequest> VolunteerRequests { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public VolunteerRequestsWriteDbContext(
        DbContextOptions<VolunteerRequestsWriteDbContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.SCHEMA);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerRequestsWriteDbContext).Assembly,
            type => type.FullName?.Contains(Constants.WRITE_DB_CONTEXT_CONFIGURATIONS) ?? false);
    }
}