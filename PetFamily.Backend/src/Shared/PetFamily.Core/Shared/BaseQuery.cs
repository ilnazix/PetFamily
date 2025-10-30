using PetFamily.Core.Abstractions;

namespace PetFamily.Core.Shared;

public record BaseQuery(
    int PageNumber,
    int PageSize) : IQuery;
