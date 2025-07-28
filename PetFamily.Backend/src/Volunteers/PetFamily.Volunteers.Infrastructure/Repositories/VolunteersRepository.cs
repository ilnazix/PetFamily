using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Application.Volunteers.Commands;
using PetFamily.Volunteers.Domain.Volunteers;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly VolunteersWriteDbContext _dbContext;

        public VolunteersRepository(VolunteersWriteDbContext dbContext)
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
    }
}
