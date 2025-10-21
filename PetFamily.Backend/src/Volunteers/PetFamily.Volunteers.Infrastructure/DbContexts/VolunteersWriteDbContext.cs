using Microsoft.EntityFrameworkCore;
using PetFamily.Volunteers.Domain.Volunteers;


namespace PetFamily.Volunteers.Infrastructure.DbContexts;

internal class VolunteersWriteDbContext : DbContext
{
    public DbSet<Volunteer> Volunteers { get; set; }

    public VolunteersWriteDbContext(DbContextOptions<VolunteersWriteDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(VolunteersWriteDbContext).Assembly,
            type => type.FullName?.Contains(Constants.WRITE_DB_CONTEXT_CONFIGURATIONS) ?? false);

        modelBuilder.HasDefaultSchema(Constants.SCHEMA);
    }
}