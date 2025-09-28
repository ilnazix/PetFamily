using PetFamily.Accounts.Application.DTOs;

namespace PetFamily.Accounts.Application.Database;

public interface IAccountsReadDbContext
{
    public IQueryable<AccountDto> Accounts { get;}
}
