using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
    public const string ROLE = "ADMIN";

    private AdminAccount() { }

    public AdminAccount(FullName fullName)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
    }

    public Guid Id { get; set; }

    public FullName FullName { get; set; }
}