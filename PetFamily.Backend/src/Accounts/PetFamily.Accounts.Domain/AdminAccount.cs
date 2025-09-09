namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
    public const string ROLE = "ADMIN";

    public AdminAccount()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
}