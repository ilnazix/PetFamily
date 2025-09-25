using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Database;
using PetFamily.Accounts.Application.DTOs;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IQueryHandler<AccountDto?, GetAccountByIdQuery>
{
    private readonly IAccountsReadDbContext _readDbContext;

    public GetAccountByIdQueryHandler(IAccountsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<AccountDto?> Handle(
        GetAccountByIdQuery query, 
        CancellationToken cancelationToken = default)
    {
        return _readDbContext.Accounts
            .FirstOrDefaultAsync(
                a => a.Id == query.Id, 
                cancelationToken);
    }
}
