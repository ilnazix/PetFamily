using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer;
using System.Runtime.CompilerServices;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VolunteersRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id;
        }

        public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

            if (result == null)
            {
                return Errors.General.NotFound(id);
            }

            return result;
        }

        public async Task<Result<Guid, Error>> Save(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            _dbContext.Volunteers.Attach(volunteer);
            await _dbContext.SaveChangesAsync();

            return volunteer.Id.Value;
        }

        public async Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            _dbContext.Volunteers.Remove(volunteer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id.Value;
        }

        public async Task DeleteExpiredSoftDeletions(TimeSpan olderThan)
        {
            var thresholdDate = DateTime.UtcNow.Subtract(olderThan);

            await _dbContext.Volunteers
                   .Where(v => v.IsDeleted && v.DeletedAt.HasValue && v.DeletedAt.Value <= thresholdDate)
                   .ExecuteDeleteAsync();
        }
    }
}
