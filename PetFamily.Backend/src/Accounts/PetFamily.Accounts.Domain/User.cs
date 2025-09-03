using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    public VolunteerAccount? VolunteerAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    public AdminAccount? AdminAccount { get; set; }
}
