using PetFamily.Volunteers.Application.DTOs;

namespace PetFamily.Volunteers.Application.Database
{
    public interface IVolunteersReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers { get; }
        public IQueryable<PetDto> Pets { get; }
    }
}
