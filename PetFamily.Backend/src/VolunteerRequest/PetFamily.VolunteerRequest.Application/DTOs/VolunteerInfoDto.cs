using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;

namespace PetFamily.VolunteerRequest.Application.DTOs;

public class VolunteerInfoDto
{
    public string PhoneNumber { get; init; } = string.Empty; 
    public string Email { get; init; } = string.Empty;
    public FullNameDto FullName { get; init; } = null!;
}