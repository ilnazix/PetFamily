using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IQuery;