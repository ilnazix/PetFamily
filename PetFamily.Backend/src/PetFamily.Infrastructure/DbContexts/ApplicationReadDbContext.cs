using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;

namespace PetFamily.Infrastructure.DbContexts
{
    public class ApplicationReadDbContext : DbContext, IReadDbContext
    {
        private readonly IConfiguration _configuration;

        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();

        public IQueryable<PetDto> Pets => Set<PetDto>();

        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

        public IQueryable<BreedDto> Breeds => Set<BreedDto>(); 

        public ApplicationReadDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(_configuration.GetConnectionString(Constants.DATABASE))
                .UseLoggerFactory(CreateLoggerFactory())
                .UseSnakeCaseNamingConvention()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationReadDbContext).Assembly, 
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
        }
    }
}
