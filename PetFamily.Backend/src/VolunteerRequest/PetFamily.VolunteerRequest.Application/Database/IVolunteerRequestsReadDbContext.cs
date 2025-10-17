using PetFamily.VolunteerRequest.Application.DTOs;

namespace PetFamily.VolunteerRequest.Application.Database;

public interface IVolunteerRequestsReadDbContext
{
    IQueryable<VolunteerRequestDto> VolunteerRequests { get; }
}
