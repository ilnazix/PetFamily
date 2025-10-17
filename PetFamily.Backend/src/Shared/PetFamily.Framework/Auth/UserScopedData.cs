namespace PetFamily.Framework.Auth;

public class UserScopedData
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;

    public bool IsAuthenticated => UserId != Guid.Empty;
}
