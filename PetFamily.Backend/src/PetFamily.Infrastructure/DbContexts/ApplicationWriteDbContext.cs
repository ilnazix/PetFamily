using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteers;
using EFCore.NamingConventions;

namespace PetFamily.Infrastructure.DbContexts
{
    public class ApplicationWriteDbContext : DbContext
    {
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Species> Species { get; set; }

        public ApplicationWriteDbContext(DbContextOptions<ApplicationWriteDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationWriteDbContext).Assembly,
                type => type.FullName?.Contains(Constants.WRITE_DB_CONTEXT_CONFIGURATIONS) ?? false);
        }
    }
}
