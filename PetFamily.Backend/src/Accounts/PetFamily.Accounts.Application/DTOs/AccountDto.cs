namespace PetFamily.Accounts.Application.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }
    public VolunteerAccountDto? VolunteerAccount { get; set; }
    public ParticipantAccountDto? ParticipantAccount { get; set; }
    public AdminAccountDto? AdminAccount { get; set; }
}