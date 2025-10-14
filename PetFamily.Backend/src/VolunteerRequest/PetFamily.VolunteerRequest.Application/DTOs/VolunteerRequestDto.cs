namespace PetFamily.VolunteerRequest.Application.DTOs;

public class VolunteerRequestDto 
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid? AdminId { get; init; }
    public VolunteerInfoDto VolunteerInfo { get; init; } = null!;
    public string Status { get; init; } = string.Empty;
    public string RejectionComment { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? RejectedAt { get; init; }
}
