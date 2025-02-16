using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetFamily.Application.Volunteers.Commands;
using PetFamily.Domain.Volunteers;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Services
{
    public class DeleteExpiredVolunteersService
    {
        private readonly ApplicationWriteDbContext _dbContext;
        private readonly VolunteerEntityOptions _options;

        public DeleteExpiredVolunteersService(ApplicationWriteDbContext volunteersRepository,
            IOptions<VolunteerEntityOptions> options)
        {
            _dbContext = volunteersRepository;
            _options = options.Value;
        }

        public async Task Process(CancellationToken cancellationToken)
        {
            var volunteers = await GetVolunteersWithPets(cancellationToken);

            foreach (var volunteer in volunteers)
            {
                var lifetimeSpan = TimeSpan.FromDays(_options.DeleteExpiredVolunteersServiceReductionDays);
                volunteer.DeleteExpiredPets(lifetimeSpan);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<IEnumerable<Volunteer>> GetVolunteersWithPets(CancellationToken cancellationToken)
        {
            return await _dbContext.Volunteers.Include(v => v.Pets).ToListAsync(cancellationToken);
        }
    }

}
