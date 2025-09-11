namespace PetFamily.Accounts.Domain;

public class RefreshSession
{
    private RefreshSession() {}

    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid RefreshToken { get; init; }
    public string UserAgent { get; init; } = string.Empty;
    public string Fingerprint { get; init; } = string.Empty;
    public string IP { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public User User { get; init; } = default!;
}
